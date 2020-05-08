using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hsk_media_server.Model;
using MediaInput;
using Transcoder;

namespace hsk_media_server.Manager
{
    public class ServerManager
    {
        private Grabber grabber = Grabber.GetSingleton();
        private FFmpegAsProcess transcoder = FFmpegAsProcess.GetSingleton();

        public String authenticate(Account account)
        {
            //TODO: check account, create Token, ...
            return "login response message";
        }

        public IEnumerable<String> getSources(String type)
        {
            return grabber.GetAvailableCategories();
        }

        public IEnumerable<ContentInformation> getMedia(String source)
        {
            return grabber.GetAvailableContentInformation(source);
        }

        public Tuple<Uri, bool> getStream(String streamId)
        {
            Tuple<Uri, bool> streamResponse = grabber.GetMediaStream(streamId);
            String ourUri = transcoder.StartProcess(streamResponse.Item1, 1, 1);
            return new Tuple<Uri, bool>(new Uri(ourUri), streamResponse.Item2);
        }

        public String getPresets()
        {
            return "stream response message";
        }

    }
}
