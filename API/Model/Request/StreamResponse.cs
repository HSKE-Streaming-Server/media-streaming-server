using System;
using System.Text.Json.Serialization;

namespace API.Model.Request
{
    public struct StreamResponse
    {
        [JsonPropertyName("stream_link")]
        public Uri StreamLink { get; set; }
        [JsonPropertyName("actual-settings")]
        public StreamSettings Settings { get; set; }
    }
}