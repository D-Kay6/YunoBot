using System;

namespace Logic.Exceptions
{
    public class InvalidBanException : Exception
    {
        public InvalidBanException()
        {
        }

        public InvalidBanException(string message) : base(message)
        {
        }

        public InvalidBanException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class BanExistsException : Exception
    {
        public BanExistsException()
        {
        }

        public BanExistsException(string message) : base(message)
        {
        }

        public BanExistsException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}