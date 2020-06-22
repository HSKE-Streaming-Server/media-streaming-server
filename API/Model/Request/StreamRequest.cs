using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Model.Request
{
    public struct StreamRequest
    {
        //[RequiredAttribute]
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [RequiredAttribute]
        [JsonPropertyName("stream_id")]
        public string StreamId { get; set; }
        [RequiredAttribute]
        [JsonPropertyName("settings")]
        public StreamSettings Settings { get; set; }
    }
}
