using System;

namespace Logic.Exceptions
{
    public class InvalidWelcomeException : Exception
    {
        public InvalidWelcomeException()
        {
        }

        public InvalidWelcomeException(string message) : base(message)
        {
        }

        public InvalidWelcomeException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}