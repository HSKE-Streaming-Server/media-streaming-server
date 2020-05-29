using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Transcoder
{
    public class TranscoderCachingObject
    {
        public TranscoderCachingObject(Uri videoSource, int audioPresetId, int videoPresetId, Uri videoTranscodedUri, CancellationTokenSource cancellationTokenSource)
        {
            VideoSourceUri = videoSource;
            AudioPresetID = audioPresetId;
            VideoPresetID = videoPresetId;
            TranscodedVideoUri = videoTranscodedUri;
            CancellationTokenSource = cancellationTokenSource;
            KeepAliveTimeStamp = DateTime.Now;
        }

        public Uri VideoSourceUri { get; }
        public int AudioPresetID { get; }
        public int VideoPresetID { get; }
        public Uri TranscodedVideoUri { get; }
        public CancellationTokenSource CancellationTokenSource {get;}
        public DateTime KeepAliveTimeStamp { get; set; }
    }
}
