using System;
using System.Collections.Generic;

namespace Transcoder
{
    public interface ITranscoder
    {
        /// <summary>
        /// Contains every Active Transcoding Process
        /// </summary>
        public IList<TranscoderCachingObject> TranscoderCache { get; }

        /// <summary>
        /// Starts the transcoding process and returns the URI where the playlist file is located.
        /// </summary>
        /// <param name="uri">The URI where the content that is to be transcoded is located.</param>
        /// <param name="videoPreset">The ID of the video preset to be used for the transcoding.</param>
        /// <param name="audioPreset">The ID of the audio preset to be used for the transcoding.</param>
        /// <returns>Returns the URI where the transcoded files are, specifically the playlist file.</returns>
        public Uri StartProcess(Uri uri, int videoPreset, int audioPreset);

        
        /// <summary>
        /// Lists all the video presets that this transcoder supports for transcoding.
        /// </summary>
        /// <returns>A list of <c>VideoPreset</c> objects.</returns>
        public IEnumerable<VideoPreset> GetAvailableVideoPresets();
        
        
        /// <summary>
        /// Lists all the audio presets that this transcoder supports for transcoding.
        /// </summary>
        /// <returns>A list of <c>AudioPreset</c> objects.</returns>
        public IEnumerable<AudioPreset> GetAvailableAudioPresets();

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="requestStreamId"></param>
        /// <returns></returns>
        IEnumerable<Tuple<int, int>> GetAvailableVoDs(Uri requestStreamId);
    }
}
