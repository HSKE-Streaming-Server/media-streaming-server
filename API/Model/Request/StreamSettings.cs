using System.Text.Json.Serialization;

namespace API.Model.Request
{
    public struct StreamSettings{

        [JsonPropertyName("video")]
        public int VideoPresetId {get; set;}
        [JsonPropertyName("audio")]
        public int AudioPresetId {get; set;}
    }
}