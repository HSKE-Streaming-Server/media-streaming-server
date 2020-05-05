namespace Transcoder
{
    public class VideoPreset
    {
        internal VideoPreset(int id, string description, string resolution, string bitrate, string transcoderArguments)
        {
            Id = id;
            Description = description;
            Resolution = resolution;
            Bitrate = bitrate;
            TranscoderArguments = transcoderArguments;
        }
        
        public readonly int Id;
        public readonly string Description;
        public readonly string Resolution;
        public readonly string Bitrate;
        internal readonly string TranscoderArguments;
    }
}
