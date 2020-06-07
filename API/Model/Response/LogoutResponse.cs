using System.Text.Json.Serialization;

namespace API.Model.Response
{
    public struct LogoutResponse
    {
        internal LogoutResponse(bool success)
        {
            Success = success;
        }
        [JsonPropertyName("success")]
        public bool Success { get; }
    }
}