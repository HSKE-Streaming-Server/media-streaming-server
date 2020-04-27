using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;

namespace MediaInput
{
    //TODO: Figure out a mutex scheme together with the keepalive module of the API so we never try to open another tuner stream when we already have one running
    public class SundtekGrabber : IGrabber
    {
        private IEnumerable<IEnumerable<ContentInformation>> _cachedContentInformation = null;
        private DateTime _lastCacheTimestamp = DateTime.UnixEpoch;

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
            return new[] {"television", "radio"};
        }

        public IEnumerable<IEnumerable<ContentInformation>> GetAvailableContentInformation()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ContentInformation> GetAvailableContentInformation(string category)
        {
            var allContent = GetAvailableContentInformation();
            //SelectMany the MetaList where at least one element has category radio
            return allContent.Where(item => item.Any(item2 => item2.Category == category)).SelectMany(item => item);
        }

        public Tuple<Stream, bool> GetMediaStream(Guid contentId)
        {
            throw new NotImplementedException();
        }
    }
}