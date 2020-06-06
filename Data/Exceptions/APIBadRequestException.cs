using System;

namespace Data.Exceptions
{
    [Serializable]
    public class ApiBadRequestException : Exception
    {
        public ApiBadRequestException()
        {

        }

        public ApiBadRequestException(string message) : base(message)
        {

        }

        public ApiBadRequestException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}

