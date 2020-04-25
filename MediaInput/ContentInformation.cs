using System;

namespace MediaInput
{
    public class ContentInformation
    {
        public ContentInformation()
        {
            
        }

        public ContentInformation(Guid id, string name, string category, bool tuner, Uri image)
        {
            Id = id;
            Name = name;
            Category = category;
            TunerIsSource = tuner;
            ImageLocation = image;
        }

        public Guid Id { get; internal set; }
        public string Name { get; internal set; }
        public string Category { get; internal set; }
        public bool TunerIsSource { get; internal set; }
        public Uri ImageLocation { get; internal set; }
    }
}