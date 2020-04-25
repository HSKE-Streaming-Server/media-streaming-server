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
        /// Gets the raw byte <c>Stream</c> for the multimedia content that is identifies by the supplied contentId.
        /// </summary>
        /// <param name="contentId">The unique content identifier.</param>
        /// <returns>A raw byte <c>Stream</c> that contains the multimedia content as grabbed from the source and whether or not the stream is a tuner sourced stream.</returns>
        public Tuple<Stream, bool> GetMediaStream(Guid contentId);
    }
}