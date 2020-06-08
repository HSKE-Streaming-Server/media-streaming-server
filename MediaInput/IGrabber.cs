using System;
using System.Collections.Generic;

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
        /// <returns>A dictionary containing the category and a list of content this <c>IGrabber</c> has access to.</returns>
        public IDictionary<string, IEnumerable<ContentInformation>> GetAvailableContentInformation();
        
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

        /// <summary>
        /// Gets the detailed ContentInformation object regarding a single contentId.
        /// </summary>
        /// <param name="contentId">The content ID in question.</param>
        /// <returns>A ContentInformation object.</returns>
        ContentInformation GetDetail(string contentId);
    }
}