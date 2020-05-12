namespace Transcoder
{
    public class VideoPreset : Preset
    {
        public readonly string Resolution;
        
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
        
        
        
    }
}