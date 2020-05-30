using System;

namespace APIExceptions
{
    [Serializable]
    public class APITunerNotAvailableException : Exception
    {

        public APITunerNotAvailableException()
        {

        }

        public APITunerNotAvailableException(string message) : base(message)
        {

        }

        public APITunerNotAvailableException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}