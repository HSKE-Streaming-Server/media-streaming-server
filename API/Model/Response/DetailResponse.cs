using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json.Serialization;
using MediaInput;
using Transcoder;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global

namespace API.Model.Response
{
    public readonly struct PresetPair
    {
        [SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
        internal PresetPair(int videoPreset, int audioPreset)
        {
            VideoPreset = videoPreset;
            AudioPreset = audioPreset;
        }

        public int VideoPreset { get; }
        public int AudioPreset { get; }
    }

    public class DetailResponse : ContentInformation
    {
        internal DetailResponse(string id, string name, string category, bool tuner, bool livestream, Uri image,
            Uri content, IEnumerable<PresetPair> existingTranscodes) : base(id, name, category, tuner, livestream,
            image, content)
        {
            ExistingTranscodes = existingTranscodes;
        }

        internal DetailResponse(ContentInformation baseObject,
            IEnumerable<Tuple<int,int>> existingTranscodes) : base(baseObject.Id, baseObject.Name,
            baseObject.Category, baseObject.TunerIsSource, baseObject.Livestream, baseObject.ImageLocation,
            baseObject.ContentLocation)
        {
            ExistingTranscodes = existingTranscodes.Select(item=>new PresetPair(item.Item1, item.Item2));
        }

        public IEnumerable<PresetPair> ExistingTranscodes { get; }
    }
}