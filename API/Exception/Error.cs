using System.Text.Json.Serialization;

namespace ErrorMessage
{
    public class Error
    {
        [JsonPropertyName("error")]
        public string ErrorMessage { get; set; }
    }
}