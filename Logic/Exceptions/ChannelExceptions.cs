﻿namespace Logic.Exceptions
{
    using System;

    public class InvalidNameException : Exception
    {
        public InvalidNameException()
        {
        }

        public InvalidNameException(string message) : base(message)
        {
        }

        public InvalidNameException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class InvalidChannelException : Exception
    {
        public InvalidChannelException()
        {
        }

        public InvalidChannelException(string message) : base(message)
        {
        }

        public InvalidChannelException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    public class ChannelExistsException : Exception
    {
        public ChannelExistsException()
        {
        }

        public ChannelExistsException(string message) : base(message)
        {
        }

        public ChannelExistsException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}