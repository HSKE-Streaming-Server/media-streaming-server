using System.Text.Json.Serialization;

namespace API.Model.Request
{
    public struct StreamRequest
    {
        public string Token { get; set; }
        [JsonPropertyName("stream_id")]
        public string StreamId { get; set; }
        public StreamSettings Settings { get; set; }
    }
}