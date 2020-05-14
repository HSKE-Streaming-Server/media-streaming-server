namespace Transcoder
{
    public class AudioPreset : Preset
    {
        internal AudioPreset(int id, string name, string description, string bitrate, string transcoderArguments) : base(id,name,description,bitrate,transcoderArguments) { }
        
    }
}