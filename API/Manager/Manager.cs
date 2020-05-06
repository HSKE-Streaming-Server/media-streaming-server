using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hsk_media_server.Model;

namespace hsk_media_server.Manager
{
    public class ServerManager
    {

        public String authenticate(Account account)
        {
            //TODO: check account, create Token, ...
            return "login response message";
        }

        public String getSources(String type)
        {
            //TODO: MediaInput -> GetAvailableCategories();
            return "source response message";
        }

        public String getMedia(String source)
        {
            //TODO: MediaInput -> GetAvailableContentInformation(category)
            return "media response message";
        }

        public String getStream(String streamId)
        {
            //TODO: - MediaInput -> getMediaStream(contentId)
            //      - Transcoder -> transcode(Stream, preset)
            return "stream response message";
        }

        public String getPresets()
        {
            return "stream response message";
        }

    }
}
