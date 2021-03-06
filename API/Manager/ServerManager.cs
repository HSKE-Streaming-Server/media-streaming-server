﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Model.Request;
using API.Model.Response;
using MediaInput;
using Microsoft.Extensions.Logging;
using Transcoder;

namespace API.Manager
{
    public class ServerManager
    {
        private readonly IGrabber _grabber;
        private readonly ITranscoder _transcoder;
        private readonly ILogger<ServerManager> _logger;


        public ServerManager(ILogger<ServerManager> logger, Grabber grabber, FFmpegAsProcess transcoder)
        {
            _grabber = grabber;
            _transcoder = transcoder;
            _logger = logger;
            TranscoderCache = _transcoder.TranscoderCache;
            CheckTranscodingCache().Start();
            _logger.LogInformation($"{nameof(ServerManager)} initialized");
        }

        private IList<TranscoderCachingObject> TranscoderCache { get; set; }

        //Returns all the "categories" or media library names, we should maybe refactor that name
        public IEnumerable<String> GetSources()
        {
            _logger.LogTrace($"Getting all available categories");
            return _grabber.GetAvailableCategories();
        }

        //Returns all the available content pieces
        public IEnumerable<ContentInformation> GetMedia(string source)
        {
            _logger.LogInformation($"Getting all media from the source {source}");
            return _grabber.GetAvailableContentInformation(source);
        }

        public StreamResponse GetStream(string streamId, int videoPreset, int audioPreset)
        {
            _logger.LogInformation(
                $"Getting stream for streamId {streamId} with videoPreset {videoPreset} and audioPreset {audioPreset}");
            var streamResponse = _grabber.GetMediaStream(streamId);
            if (_transcoder.GetAvailableAudioPresets().All(item => item.PresetId != audioPreset))
            {
                var actualAudioPreset = _transcoder.GetAvailableAudioPresets().First().PresetId;
                _logger.LogWarning(
                    $"Replacing audioPreset {audioPreset} with {actualAudioPreset} because not found in transcoder");
                audioPreset = actualAudioPreset;
            }

            if (_transcoder.GetAvailableVideoPresets().All(item => item.PresetId != videoPreset))
            {
                var actualVideoPreset = _transcoder.GetAvailableVideoPresets().First().PresetId;
                _logger.LogWarning(
                    $"Replacing videoPreset {videoPreset} with {actualVideoPreset} because not found in transcoder");
                videoPreset = actualVideoPreset;
            }

            Uri ourUri = null;

            lock (TranscoderCache)
            {
                var cacheObject = TranscoderCache.FirstOrDefault(item =>
                    item.VideoSourceUri == streamResponse.Item1 && item.AudioPresetId == audioPreset &&
                    item.VideoPresetId == videoPreset);
                if (cacheObject != null)
                    ourUri = cacheObject.TranscodedVideoUri;
            }

            ourUri ??= _transcoder.StartProcess(streamResponse.Item1, videoPreset, audioPreset);
            return new StreamResponse()
            {
                Settings = new StreamSettings()
                {
                    AudioPresetId = audioPreset,
                    VideoPresetId = videoPreset
                },
                StreamLink = ourUri
            };
        }

        public IEnumerable<VideoPreset> GetVideoPresets()
        {
            return _transcoder.GetAvailableVideoPresets();
        }

        public IEnumerable<AudioPreset> GetAudioPresets()
        {
            return _transcoder.GetAvailableAudioPresets();
        }

        public void KeepAlive(KeepAliveRequest request)
        {
            lock (TranscoderCache)
            {
                var cacheObject = TranscoderCache.FirstOrDefault(item =>
                    item.TranscodedVideoUri == request.TranscodedVideoUri);
                if (cacheObject != null)
                {
                    cacheObject.KeepAliveTimeStamp = DateTime.Now;
                }

                //TODO: Response to Client? Keepalive Invalid, Video stopped to transcode
            }
        }

        private Task CheckTranscodingCache()
        {
            Task checkCache = new Task(async () =>
            {
                while (true)
                {
                    //Locked for operations
                    lock (TranscoderCache)
                    {
                        if (TranscoderCache.Count == 0)
                            continue;

                        //Cant delete IEnumerable Entries while in for each
                        var iterationList = new List<TranscoderCachingObject>();
                        iterationList.InsertRange(0, TranscoderCache);

                        foreach (var video in iterationList)
                        {
                            if ((DateTime.Now - video.KeepAliveTimeStamp) > TimeSpan.FromMinutes(1))
                            {
                                _logger.LogInformation(
                                    $"Keepalive for Video {video.VideoSourceUri} with videoPreset {video.VideoPresetId} and audioPreset {video.AudioPresetId} expired");
                                //Deactivate Token to stop Process
                                video.CancellationTokenSource.Cancel();
                            }

                            if (!video.CancellationTokenSource.IsCancellationRequested) continue;
                            //Delete Entry in real Cache
                            _logger.LogInformation(
                                $"Process for Video {video.VideoSourceUri} with videoPreset {video.VideoPresetId} and audioPreset {video.AudioPresetId} removed from Cache");
                            TranscoderCache.Remove(video);
                            video.CancellationTokenSource.Dispose();
                        }
                    }

                    //Using Task.Delay instead of Thread.Sleep which is better for the Thread Pool
                    await Task.Delay(5000);
                }

                // ReSharper disable once FunctionNeverReturns
            });
            return checkCache;
        }

        public DetailResponse GetDetail(DetailRequest request)
        {
            var contentInformation = _grabber.GetDetail(request.StreamId);
            var vodInformation = _transcoder.GetAvailableVoDs(contentInformation.ContentLocation);
            return new DetailResponse(contentInformation, vodInformation);
        }
    }
}