using System;
using System.Diagnostics.CodeAnalysis;

namespace MediaInput
{
    public class ContentInformation
    {
        public Guid Id { get; internal set; }
        public string Name { get; internal set; }
        public string Category { get; internal set; }
        public bool TunerIsSource { get; internal set; }
        public Uri ImageLocation { get; internal set; }
    }
}