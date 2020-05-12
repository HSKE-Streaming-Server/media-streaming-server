using System;
using System.Collections.Generic;
using API.Model;
using MediaInput;
using Transcoder;

namespace API.Manager
{
    public class ServerManager
    {
        private readonly Grabber _grabber = Grabber.GetSingleton();
        private readonly FFmpegAsProcess _transcoder = FFmpegAsProcess.GetSingleton();

        public string Authenticate(Account account)
        {
            //TODO: check account, create Token, ...
            return "login response message";
        }

        //Returns all the "categories" or media library names, we should maybe refactor that name
        public IEnumerable<String> GetSources(string type)
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

        public static string GetPresets()
        {
            return "stream response message";
        }

    }
}
