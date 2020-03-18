namespace IDal.Exceptions
{
    using System;

    public class InvalidItemException : Exception
    {
        public InvalidItemException()
        {
        }

        public InvalidItemException(string message) : base(message)
        {
        }

        public InvalidItemException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}