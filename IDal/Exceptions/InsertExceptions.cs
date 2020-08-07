using System;

namespace IDal.Exceptions
{
    public class ItemExistsException : Exception
    {
        public ItemExistsException()
        {
        }

        public ItemExistsException(string message) : base(message)
        {
        }

        public ItemExistsException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}