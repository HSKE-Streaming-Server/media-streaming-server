using System;

namespace Data.Exceptions
{
    [Serializable]
    public class ApiNotFoundException : Exception
    {

        public ApiNotFoundException()
        {

        }

        public ApiNotFoundException(string message) : base(message)
        {

        }

        public ApiNotFoundException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}