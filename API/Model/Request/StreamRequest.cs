using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Model.Request
{
    public struct StreamRequest
    {
        //[RequiredAttribute]
        public string Token { get; set; }
        [RequiredAttribute]
        [JsonPropertyName("stream_id")]
        public string StreamId { get; set; }
        [RequiredAttribute]
        public StreamSettings Settings { get; set; }
    }
}
