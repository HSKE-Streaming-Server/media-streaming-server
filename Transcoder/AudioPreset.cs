namespace Transcoder
{
    public class AudioPreset
    {
        internal AudioPreset(int id, string name, string description, string bitrate, string transcoderArguments)
        {
            Id = id;
            Name = name;
            Description = description;
            Bitrate = bitrate;
            TranscoderArguments = transcoderArguments;
        }
        
        public readonly int Id;
        public readonly string Name;
        public readonly string Description;
        public readonly string Bitrate;
        internal readonly string TranscoderArguments;
    }
}