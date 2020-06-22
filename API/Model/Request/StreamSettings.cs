using System.Text.Json.Serialization;

namespace API.Model.Request
{
    public struct StreamSettings{

        [JsonPropertyName("videoPresetId")]
        public int VideoPresetId {get; set;}
        [JsonPropertyName("audioPresetId")]
        public int AudioPresetId {get; set;}
    }
}