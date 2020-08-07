using System;

namespace Logic.Exceptions
{
    public class InvalidServerException : Exception
    {
        public InvalidServerException()
        {
        }

        public InvalidServerException(string message) : base(message)
        {
        }

        public InvalidServerException(string message, Exception inner) : base(message, inner)
        {
        }
    }
    public class UnknownServerException : Exception
    {
        public UnknownServerException()
        {
        }

        public UnknownServerException(string message) : base(message)
        {
        }

        public UnknownServerException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}