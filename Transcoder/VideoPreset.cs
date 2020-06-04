using System.Text.Json.Serialization;

namespace Transcoder
{
    public class VideoPreset : Preset
    {
        // ReSharper disable MemberCanBePrivate.Global
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        [JsonPropertyName("resolutionX")]
        public int ResolutionX { get; }
        [JsonPropertyName("resolutionY")]
        public int ResolutionY { get; }
        
        internal VideoPreset(int presetId, string displayName, string description, int resX, int resY, int bitrate,
            string transcoderArguments) : base(presetId, displayName, description, bitrate, transcoderArguments)
        {
            ResolutionX = resX;
            ResolutionY = resY;
        }
        
    }
}