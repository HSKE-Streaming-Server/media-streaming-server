using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Transcoder
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public abstract class Preset
    {
        [JsonPropertyName("presetID")]
        public int PresetId { get; private set; }
        [JsonPropertyName("displayName")]
        public string DisplayName { get; private set; }
        [JsonPropertyName("description")]
        public string Description { get; private set; }
        [JsonPropertyName("bitrate")]
        public int Bitrate { get; private set; }
        internal string TranscoderArguments { get; private set; }

        protected Preset(int presetid, string displayname, string description, int bitrate, string transcoderArguments)
        {
            PresetId = presetid;
            DisplayName = displayname;
            Description = description;
            Bitrate = bitrate;
            TranscoderArguments = transcoderArguments;
        }
       
    }
}