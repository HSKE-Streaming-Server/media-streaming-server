namespace Transcoder
{
    public class AudioPreset : Preset
    {
        internal AudioPreset(int id, string name, string description, string bitrate, string transcoderArguments)
        {
            Id = id;
            Name = name;
            Description = description;
            Bitrate = bitrate;
            TranscoderArguments = transcoderArguments;
        }
    }
}