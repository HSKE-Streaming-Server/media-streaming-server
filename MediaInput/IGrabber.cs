using System;
using System.Collections.Generic;
using System.IO;

namespace MediaInput
{
    public interface IGrabber
    {
        /// <summary>
        /// Gets all the categories this <c>IGrabber</c> has access to.
        /// </summary>
        /// <returns>A list of all the categories this <c>IGrabber</c> has access to.</returns>
        public IEnumerable<string> GetAvailableCategories();
        
        /// <summary>
        /// Gets all the content this <c>IGrabber</c> has access to.
        /// </summary>
        /// <returns>A two-dimensional list of all the categories and containing content this <c>IGrabber</c> has access to.</returns>
        public IEnumerable<IEnumerable<ContentInformation>> GetAvailableContentInformation();
        
        /// <summary>
        /// Gets all the content in a specific category this <c>IGrabber</c> has access to.
        /// </summary>
        /// <param name="category">The name of the category.</param>
        /// <returns>A list of all the content this <c>IGrabber</c> has access to.</returns>
        public IEnumerable<ContentInformation> GetAvailableContentInformation(string category);
        
        /// <summary>
        /// Gets the URI where the multimedia content is located that is identifies by the supplied contentId.
        /// </summary>
        /// <param name="contentId">The unique content identifier.</param>
        /// <returns>The URI where the multimedia content is located and whether or not the stream is a tuner sourced stream.</returns>
        public Tuple<Uri, bool> GetMediaStream(string contentId);
    }
}