using System;

namespace APIExceptions
{
    [Serializable]
    public class APINotFoundException : Exception
    {

        public APINotFoundException()
        {

        }

        public APINotFoundException(string message) : base(message)
        {

        }

        public APINotFoundException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}