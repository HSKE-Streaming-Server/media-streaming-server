using System;
using System.Threading;

namespace Transcoder
{
    public class TranscoderCachingObject
    {
        public TranscoderCachingObject(Uri videoSource, int audioPresetId, int videoPresetId, Uri videoTranscodedUri, CancellationTokenSource cancellationTokenSource)
        {
            VideoSourceUri = videoSource;
            AudioPresetId = audioPresetId;
            VideoPresetId = videoPresetId;
            TranscodedVideoUri = videoTranscodedUri;
            CancellationTokenSource = cancellationTokenSource;
            KeepAliveTimeStamp = DateTime.Now;
        }

        public Uri VideoSourceUri { get; }
        public int AudioPresetId { get; }
        public int VideoPresetId { get; }
        public Uri TranscodedVideoUri { get; }
        public CancellationTokenSource CancellationTokenSource {get;}
        public DateTime KeepAliveTimeStamp { get; set; }
    }
}
