using System.Diagnostics.CodeAnalysis;

namespace Transcoder
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public abstract class Preset
    {
        public int presetID { get; private set; }
        public string displayName { get; private set; }
        public string Description { get; private set; }
        public int bitrate { get; private set; }
        internal string TranscoderArguments { get; private set; }

        protected Preset(int presetid, string displayname, string description, int bitrate, string transcoderArguments)
        {
            presetID = presetid;
            displayName = displayname;
            Description = description;
            this.bitrate = bitrate;
            TranscoderArguments = transcoderArguments;
        }
       
    }
}