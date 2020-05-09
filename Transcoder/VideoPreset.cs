namespace Transcoder
{
    public class VideoPreset
    {
        internal VideoPreset(int id, string name, string description, string resolution, string bitrate,
            string transcoderArguments)
        {
            Id = id;
            Name = name;
            Description = description;
            Resolution = resolution;
            Bitrate = bitrate;
            TranscoderArguments = transcoderArguments;
        }

        public readonly int Id;
        public readonly string Name;
        public readonly string Description;
        public readonly string Resolution;
        public readonly string Bitrate;
        internal readonly string TranscoderArguments;
    }
}