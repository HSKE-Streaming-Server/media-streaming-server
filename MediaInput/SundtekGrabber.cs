using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace MediaInput
{
    //TODO: Figure out a mutex scheme together with the keepalive module of the API so we never try to open another tuner stream when we already have one running
    public class SundtekGrabber : IGrabber
    {
        //Singleton pattern
        private static SundtekGrabber _singleton = null;

        static SundtekGrabber()
        {
            _singleton = new SundtekGrabber();
        }
        public static SundtekGrabber GetSingleton()
        {
            //TODO: how do we pass the HTTPClient to this class? 
            //returns _singleton if its not null, otherwise creates new instance and assigns it to _singleton
            return _singleton ??= new SundtekGrabber();
        }

        private SundtekGrabber()
        {
            
        }
        
        
        public IEnumerable<string> GetAvailableCategories()
        {
            return new[] {"TV", "Radio"};
        }

        public IEnumerable<IEnumerable<ContentInformation>> GetAvailableContentInformation()
        {
            //TODO: Implement this by getting the playlist files from the sundtek server and "parsing" the .m3u files
            throw new NotImplementedException();
        }

        public IEnumerable<ContentInformation> GetAvailableContentInformation(string category)
        {
            throw new NotImplementedException();
        }

        public Stream GetMediaStream(Guid contentId)
        {
            throw new NotImplementedException();
        }
    }
}