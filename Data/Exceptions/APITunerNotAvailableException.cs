using System;

namespace Data.Exceptions
{
    [Serializable]
    public class ApiTunerNotAvailableException : Exception
    {

        public ApiTunerNotAvailableException()
        {

        }

        public ApiTunerNotAvailableException(string message) : base(message)
        {

        }

        public ApiTunerNotAvailableException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}