using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Model;
using API.Model.Request;
using API.Login;
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
        private readonly LoginDbHandler _loginDbHandler;

        private readonly Dictionary<string, (string, DateTime)> _token; //string 1 = token, string 2 = username, Datime = timestamp of when the token was created/valdiated

        public ServerManager(ILogger<ServerManager> logger, Grabber grabber, FFmpegAsProcess transcoder, LoginDbHandler loginDbHandler)
        {
            _loginDbHandler = loginDbHandler;
            _grabber = grabber;
            _transcoder = transcoder;
            _logger = logger;

            _token = new Dictionary<string, (string, DateTime)>(); 

            TranscoderCache = _transcoder.TranscoderCache;
            CheckTranscodingCache().Start();
            _logger.LogInformation($"{nameof(ServerManager)} initialized");
        }

        private IList<TranscoderCachingObject> TranscoderCache { get; set; }


        public string Login(Account account)
        {

            _loginDbHandler.CreateToken(account);
           
            //in der Datenbank nachschauen ob daten valid sind und wenn ja, dann dictonary eintrag mit aktuellem timestamp erstellen


            return "was geht ab";

        }

        /// <summary>
        /// Checks whether or not a token is still valid for use and automatically revalidates it, if it is still valid.
        /// </summary>
        /// <param name="token">The token that is to be checked.</param>
        /// <returns>Whether or not the token is valid.</returns>
        public bool CheckValidityOfToken(string token)
        {
            _logger.LogInformation($"Checking validity of token {token}");
            RevalidateToken(token, _defaultTimespan);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Revalidates a token for a certain period.
        /// </summary>
        /// <param name="token">The token that is to be revalidated.</param>
        /// <param name="validityPeriod">The period the token should be revalidated for</param>

        private void RevalidateToken(string token, TimeSpan validityPeriod)
        {
            _logger.LogInformation($"Revalidating token {token} for {validityPeriod:g}");
            throw new NotImplementedException();
        }

        /// <summary>
        /// Get the username for a supplied token.
        /// </summary>
        /// <param name="token">The token that was supplied in the request.</param>
        /// <returns>The username of the user account this token currently belongs to.</returns>
        /// <exception cref="KeyNotFoundException">The token was either not found or is invalid.</exception>
        public string GetUsernameForToken(string token)
        {
            _logger.LogInformation($"Finding user for token {token}");
            //TODO: check account, create Token, ...
            throw new NotImplementedException();
        }

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
            _logger.LogInformation($"Getting stream for streamId {streamId} with videoPreset {videoPreset} and audioPreset {audioPreset}");
            var streamResponse = _grabber.GetMediaStream(streamId);
            if (_transcoder.GetAvailableAudioPresets().All(item => item.presetID != audioPreset))
            {
                var actualAudioPreset = _transcoder.GetAvailableAudioPresets().First().presetID;
                _logger.LogWarning($"Replacing audioPreset {audioPreset} with {actualAudioPreset} because not found in transcoder");
                audioPreset = actualAudioPreset;
            }
            if (_transcoder.GetAvailableVideoPresets().All(item => item.presetID != videoPreset))
            {
                var actualVideoPreset = _transcoder.GetAvailableVideoPresets().First().presetID;
                _logger.LogWarning($"Replacing videoPreset {videoPreset} with {actualVideoPreset} because not found in transcoder");
                videoPreset = actualVideoPreset;
            }
            Uri ourUri = null;

            lock (TranscoderCache)
            {
                var cacheObject = TranscoderCache.FirstOrDefault(item => item.VideoSourceUri == streamResponse.Item1 && item.AudioPresetID == audioPreset && item.VideoPresetID == videoPreset);
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
                var cacheObject = TranscoderCache.FirstOrDefault(item => item.TranscodedVideoUri == request.TranscodedVideoUri &&
                    item.AudioPresetID == request.AudioPreset &&
                    item.VideoPresetID == request.VideoPreset);
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
                        var IterationList = new List<TranscoderCachingObject>();
                        IterationList.InsertRange(0, TranscoderCache);

                        foreach (var video in IterationList)
                        {
                            if ((DateTime.Now - video.KeepAliveTimeStamp) > TimeSpan.FromMinutes(1))
                            {
                                _logger.LogInformation($"Keepalive for Video {video.VideoSourceUri} with videoPreset {video.VideoPresetID} and audioPreset {video.AudioPresetID} expired");
                                //Deactivate Token to stop Process
                                video.CancellationTokenSource.Cancel();
                            }
                            if (video.CancellationTokenSource.IsCancellationRequested)
                            {
                                //Delete Entry in real Cache
                                _logger.LogInformation($"Process for Video {video.VideoSourceUri} with videoPreset {video.VideoPresetID} and audioPreset {video.AudioPresetID} removed from Cache");
                                TranscoderCache.Remove(video);
                                video.CancellationTokenSource.Dispose();
                            }
                        }
                        IterationList = null;
                    }
                    //Using Task.Delay instead of Thread.Sleep which is better for the Thread Pool
                    await Task.Delay(5000);
                }
            });
            return checkCache;
        }
    }
}
