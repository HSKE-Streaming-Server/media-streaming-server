using System;
using System.Text.Json.Serialization;

namespace MediaInput
{
    public class ContentInformation
    {

        internal ContentInformation(string id, string name, string category, bool tuner, bool livestream, Uri image, Uri content, string description = null)
        {
            Id = id;
            Name = name;
            Category = category;
            TunerIsSource = tuner;
            Livestream = livestream; 
            ImageLocation = image;
            ContentLocation = content;
            Description = description;
        }

        public string Id { get; internal set; }
        public string Name { get; internal set; }
        public string Category { get; internal set; }
        public bool TunerIsSource { get; internal set; }
        public bool Livestream { get; internal set; }
        [JsonPropertyName("image")]
        public Uri ImageLocation { get; internal set; }
        internal Uri ContentLocation { get; set; }
        public string Description { get; set; }
    }
}