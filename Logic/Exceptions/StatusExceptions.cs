using System;

namespace Logic.Exceptions
{
    public class InvalidStatusException : Exception
    {
        public InvalidStatusException()
        {
        }

        public InvalidStatusException(string message) : base(message)
        {
        }

        public InvalidStatusException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class StatusExistsException : Exception
    {
        public StatusExistsException()
        {
        }

        public StatusExistsException(string message) : base(message)
        {
        }

        public StatusExistsException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}