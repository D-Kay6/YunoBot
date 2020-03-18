namespace Logic.Exceptions
{
    using System;

    public class InvalidCommandException : Exception
    {
        public InvalidCommandException()
        {
        }

        public InvalidCommandException(string message) : base(message)
        {
        }

        public InvalidCommandException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class CommandExistsException : Exception
    {
        public CommandExistsException()
        {
        }

        public CommandExistsException(string message) : base(message)
        {
        }

        public CommandExistsException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}