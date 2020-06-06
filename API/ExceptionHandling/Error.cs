using System.Text.Json.Serialization;

namespace API.ExceptionHandling
{
    public class Error
    {
        [JsonPropertyName("error")]
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string ErrorMessage { get; set; }
    }
}