using System;

namespace Logic.Exceptions
{
    public class InvalidPrefixException : Exception
    {
        public InvalidPrefixException()
        {
        }

        public InvalidPrefixException(string message) : base(message)
        {
        }

        public InvalidPrefixException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class PrefixExistsException : Exception
    {
        public PrefixExistsException()
        {
        }

        public PrefixExistsException(string message) : base(message)
        {
        }

        public PrefixExistsException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}