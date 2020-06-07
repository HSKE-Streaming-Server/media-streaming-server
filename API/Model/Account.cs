

using System.Text.Json.Serialization;

namespace API.Model
{
    public class Account{

        [JsonPropertyName("username")]
        public string Username {get; set;}
        [JsonPropertyName("password")]
        public string Password {get; set;}

    }
}