using System;
using System.Text.Json.Serialization;
using API.Model.Request;

namespace API.Model.Response
{
    public struct StreamResponse
    {
        [JsonPropertyName("stream_link")]
        public Uri StreamLink { get; set; }
        [JsonPropertyName("actual-settings")]
        public StreamSettings Settings { get; set; }
    }
}