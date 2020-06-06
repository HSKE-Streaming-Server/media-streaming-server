using System.Text.Json.Serialization;

namespace API.Model.Response
{
    public struct LoginResponse
    {
        internal LoginResponse(string token)
        {
            Success = true;
            Userdata = new TokenStruct {Token = token};
        }
        [JsonPropertyName("succes")]
        public bool Success { get; }

        [JsonPropertyName("userdata")]
        public TokenStruct Userdata { get; }
        
        
        public struct TokenStruct
        {
            [JsonPropertyName("token")]
            public string Token { get; internal set; }
        }
    }
}
