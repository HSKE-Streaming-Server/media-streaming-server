using System;

namespace Transcoder
{
    public class FFmpegProcessException : Exception
    {
        public string Parameters;
        public string WorkingDirectory;
        internal FFmpegProcessException(string parameters, string workingDirectory, string message) : base(message)
        {
            Parameters = parameters;
            WorkingDirectory = workingDirectory;
        }

        internal FFmpegProcessException(Exception innerException, string parameters, string workingDirectory,
            string message) : base(message, innerException)
        {
            Parameters = parameters;
            WorkingDirectory = workingDirectory;
        }
    }
}