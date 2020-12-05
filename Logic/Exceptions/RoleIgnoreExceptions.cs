using System;

namespace Logic.Exceptions
{
    public class InvalidRoleIgnoreException : Exception
    {
        public InvalidRoleIgnoreException()
        {
        }

        public InvalidRoleIgnoreException(string message) : base(message)
        {
        }

        public InvalidRoleIgnoreException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class RoleIgnoreExistsException : Exception
    {
        public RoleIgnoreExistsException()
        {
        }

        public RoleIgnoreExistsException(string message) : base(message)
        {
        }

        public RoleIgnoreExistsException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}