using System;
using System.Collections.Generic;
using System.Linq;
using API.Model;
using API.Model.Request;
using MediaInput;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.X509;
using Transcoder;

namespace API.Manager
{
    public class ServerManager
    {
        private readonly IGrabber _grabber;
        private readonly ITranscoder _transcoder;
        private readonly ILogger<ServerManager> _logger;
        
        private static readonly TimeSpan _defaultTimespan = TimeSpan.FromMinutes(30);
        public ServerManager(ILogger<ServerManager> logger, Grabber grabber, FFmpegAsProcess transcoder)
        {
            _grabber = grabber;
            _transcoder = transcoder;
            _logger = logger;
            _logger.LogInformation($"{nameof(ServerManager)} initialized");
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
                var actualVideoPreset =_transcoder.GetAvailableVideoPresets().First().presetID;
                _logger.LogWarning($"Replacing videoPreset {videoPreset} with {actualVideoPreset} because not found in transcoder");
                videoPreset = actualVideoPreset;
            }
            var ourUri = _transcoder.StartProcess(streamResponse.Item1, videoPreset, audioPreset);
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
    }
}
