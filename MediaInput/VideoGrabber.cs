using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MediaInput
{
    public class VideoGrabber : IGrabber
    {
        //Singleton
        private static VideoGrabber _singleton = null;

        static VideoGrabber()
        {
            _singleton = new VideoGrabber();
        }

        public static VideoGrabber GetSingletonInstance()
        {
            return _singleton ??= new VideoGrabber();
        }

        public IEnumerable<string> GetAvailableCategories()
        {
            //TODO: Categories? Sind hierbei die Anbieter (ZDF etc) gemeint, oder fällt dies für den Videograbber flach? (vgl. Sundtekgrabber - TV und Radio)
            throw new NotImplementedException();
        }

        public IEnumerable<IEnumerable<ContentInformation>> GetAvailableContentInformation()
        {
            //TODO: Setup SqlConnection first
            //Call GetAvailableContentInformation(string category) Foreach Category in GetAvailableCategories - probably bad performance (1 Iteration through Database foreach Category)
            //Better: Create ContentInformation for every DatabaseRow and Add to spezific List - Problem: Category Count unknown?
            throw new NotImplementedException();
        }

        public IEnumerable<ContentInformation> GetAvailableContentInformation(string category)
        {
            //Call GetAvailableContentInformation() and choose list where category == category of elements
            throw new NotImplementedException();
        }

        public Stream GetMediaStream(Guid contentId)
        {
            //TODO: Wait for Niklas Merge on Master - Updating ContentInformation and IGrabber
            throw new NotImplementedException();
        }
    }
}
