using System.Text.Json.Serialization;

namespace API.Model.Response
{
    public struct AuthenticateResponse
    {
        internal AuthenticateResponse(string username)
        {
            Success = true;
            Userdata = new UsernameStruct {Username = username};
        }
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        [JsonPropertyName("userdata")]
        public UsernameStruct Userdata { get; set; }

        public struct UsernameStruct
        {
            [JsonPropertyName("username")]
            public string Username { get; set; }
        }
    }
}