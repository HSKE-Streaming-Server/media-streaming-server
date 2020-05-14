namespace Transcoder
{
    public class AudioPreset : Preset
    {
        internal AudioPreset(int presetId, string displayName, string description, int bitrate, string transcoderArguments) : base(presetId,displayName,description,bitrate,transcoderArguments) { }
        
    }
}