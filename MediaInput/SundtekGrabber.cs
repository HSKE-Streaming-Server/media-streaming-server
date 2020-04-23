using System;
using System.Collections.Generic;
using System.IO;

namespace MediaInput
{
    public class SundtekGrabber : IGrabber
    {
        //Singleton pattern
        static private SundtekGrabber _singleton = null;
        static public SundtekGrabber GetSingleton => new SundtekGrabber();

        private SundtekGrabber()
        {
            
        }
        public IEnumerable<string> GetAvailableCategories()
        {
            return new[] {"TV", "Radio"};
        }

        public IEnumerable<IEnumerable<ContentInformation>> GetAvailableContentInformation()
        {
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