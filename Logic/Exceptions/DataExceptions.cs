using System;

namespace Logic.Exceptions
{
    public class DataExistsException : Exception
    {
        public DataExistsException()
        {
        }

        public DataExistsException(string message) : base(message)
        {
        }

        public DataExistsException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class DataIncompleteException : Exception
    {
        public DataIncompleteException()
        {
        }

        public DataIncompleteException(string message) : base(message)
        {
        }

        public DataIncompleteException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}