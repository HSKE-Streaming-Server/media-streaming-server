namespace Transcoder
{
    public class AudioPreset
    {
        internal AudioPreset(int id, string description, string bitrate, string transcoderArguments)
        {
            Id = id;
            Description = description;
            Bitrate = bitrate;
            TranscoderArguments = transcoderArguments;
        }
        
        public readonly int Id;
        public readonly string Description;
        public readonly string Bitrate;
        internal readonly string TranscoderArguments;
    }
}