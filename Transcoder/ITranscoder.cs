using System;

namespace Transcoder
{
    public interface ITranscoder
    {
        /// <summary>
        /// APIManager (Felix) passes an URI to transcoder
        /// </summary>
        /// <param name="uri">input</param>
        /// <returns>Returns the uri where the transcoded files are.</returns>
        public string startProcess(Uri uri);

        //Presetsinformationen an APIManager zurückgeben
    }
}
