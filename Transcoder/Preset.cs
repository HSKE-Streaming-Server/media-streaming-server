using System.Diagnostics.CodeAnalysis;

namespace Transcoder
{
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    public abstract class Preset
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string Bitrate { get; private set; }
        internal string TranscoderArguments { get; private set; }

        protected Preset(int id, string name, string description, string bitrate, string transcoderArguments)
        {
            Id = id;
            Name = name;
            Description = description;
            Bitrate = bitrate;
            TranscoderArguments = transcoderArguments;
        }
       
    }
}