using System.Text.Json.Serialization;

namespace API.Model.Request
{
    public struct DetailRequest
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("streamId")]
        public string StreamId { get; set; }
    }
}