using System;
using System.Collections.Generic;
using API.Model;
using MediaInput;
using Transcoder;

namespace API.Manager
{
    public class ServerManager
    {
        private readonly IGrabber _grabber = Grabber.GetSingleton();
        private readonly ITranscoder _transcoder = FFmpegAsProcess.GetSingleton();
        private static readonly TimeSpan _defaultTimespan = TimeSpan.FromMinutes(30);

        /// <summary>
        /// Checks whether or not a token is still valid for use and automatically revalidates it, if it is still valid.
        /// </summary>
        /// <param name="token">The token that is to be checked.</param>
        /// <returns>Whether or not the token is valid.</returns>
        public bool CheckValidityOfToken(string token)
        {
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
            //TODO: check account, create Token, ...
            throw new NotImplementedException();
        }

        //Returns all the "categories" or media library names, we should maybe refactor that name
        public IEnumerable<String> GetSources()
        {
            return _grabber.GetAvailableCategories();
        }
        //Returns all the available content pieces
        public IEnumerable<ContentInformation> GetMedia(string source)
        {
            return _grabber.GetAvailableContentInformation(source);
        }

        public Tuple<Uri, bool> GetStream(string streamId, int videoPreset, int audioPreset)
        {
            Tuple<Uri, bool> streamResponse = _grabber.GetMediaStream(streamId);
            string ourUri = _transcoder.StartProcess(streamResponse.Item1, videoPreset, audioPreset);
            return new Tuple<Uri, bool>(new Uri(ourUri), streamResponse.Item2);
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
