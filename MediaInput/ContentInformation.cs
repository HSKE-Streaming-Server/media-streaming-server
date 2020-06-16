using System;
using System.Text.Json.Serialization;

namespace MediaInput
{
    public class ContentInformation
    {
        protected internal ContentInformation(string id, string name, string category, bool tuner, bool livestream, Uri image, Uri content)
        {
            Id = id;
            Name = name;
            Category = category;
            TunerIsSource = tuner;
            Livestream = livestream; 
            ImageLocation = image;
            ContentLocation = content;
        }

        public string Id { get; internal set; }
        public string Name { get; internal set; }
        public string Category { get; internal set; }
        public bool TunerIsSource { get; internal set; }
        public bool Livestream { get; internal set; }
        [JsonPropertyName("image")]
        public Uri ImageLocation { get; internal set; }
        [JsonIgnore]
        public Uri ContentLocation { get; set; }
    }
}