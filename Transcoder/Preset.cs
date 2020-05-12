namespace Transcoder
{
    public abstract class Preset
    {
        public int Id;
        public string Name;
        public string Description;
        public string Bitrate;
        internal string TranscoderArguments;
    }
}