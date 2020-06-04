using System;

namespace APIExceptions
{
    [Serializable]
    public class APIUnauthorizedException : Exception
    {

        public APIUnauthorizedException()
        {

        }

        public APIUnauthorizedException(string message) : base(message)
        {

        }

        public APIUnauthorizedException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}