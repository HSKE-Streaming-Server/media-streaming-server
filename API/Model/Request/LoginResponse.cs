using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace API.Model.Request
{


    public struct LoginResponse
    {
        [JsonPropertyName("succes")]
        public Boolean Success { get; set; }

        [JsonPropertyName("userdata")]
        public UserDataStruct Userdata { get; set; }
        

    }
}

namespace API.Model.Request
{
    public struct UserDataStruct
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}
