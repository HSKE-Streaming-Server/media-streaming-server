namespace Transcoder
{
    public class VideoPreset : Preset
    {
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public string Resolution { get; private set; }
        
        internal VideoPreset(int id, string name, string description, string resolution, string bitrate,
            string transcoderArguments) : base(id, name, description, bitrate, transcoderArguments)
        {
            Resolution = resolution;
        }
        
    }
}