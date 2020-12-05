using System;

namespace Logic.Exceptions
{
    public class InvalidSessionException : Exception
    {
        public InvalidSessionException()
        {
        }

        public InvalidSessionException(string message) : base(message)
        {
        }

        public InvalidSessionException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class SessionExistsException : Exception
    {
        public SessionExistsException()
        {
        }

        public SessionExistsException(string message) : base(message)
        {
        }

        public SessionExistsException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}