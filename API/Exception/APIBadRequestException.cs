using System;

namespace APIExceptions
{
    [Serializable]
    public class APIBadRequestException : Exception
    {
        public APIBadRequestException()
        {

        }

        public APIBadRequestException(string message) : base(message)
        {

        }

        public APIBadRequestException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}

