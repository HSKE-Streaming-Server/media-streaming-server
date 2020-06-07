using System.Text.Json.Serialization;

namespace API.Model.Request
{
    public struct TokenOnlyRequest
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}