using System;

namespace API.Model.Request
{
    public class KeepAliveRequest
    {
        public string Token { get; set; }
        public int AudioPreset { get; set; }
        public int VideoPreset { get; set; }
        public Uri TranscodedVideoUri { get; set; }
    }
}
