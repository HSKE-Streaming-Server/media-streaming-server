namespace Transcoder
{
    public class VideoPreset : Preset
    {
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public int resolutionX { get; set; }
        public int resolutionY { get; set; }
        
        internal VideoPreset(int presetId, string displayName, string description, int resX, int resY, int bitrate,
            string transcoderArguments) : base(presetId, displayName, description, bitrate, transcoderArguments)
        {
            resolutionX = resX;
            resolutionY = resY;
        }
        
    }
}