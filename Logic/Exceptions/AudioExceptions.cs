using System;

namespace Logic.Exceptions
{
    public class InvalidPlayerException : Exception
    {
        public InvalidPlayerException() { }

        public InvalidPlayerException(string message) : base(message) { }

        public InvalidPlayerException(string message, Exception inner) : base(message, inner) { }
    }

    public class InvalidChannelException : Exception
    {
        public InvalidChannelException() { }

        public InvalidChannelException(string message) : base(message) { }

        public InvalidChannelException(string message, Exception inner) : base(message, inner) { }
    }

    public class InvalidTrackException : Exception
    {
        public InvalidTrackException() { }

        public InvalidTrackException(string message) : base(message) { }

        public InvalidTrackException(string message, Exception inner) : base(message, inner) { }
    }

    public class InvalidFormatException : Exception
    {
        public InvalidFormatException() { }

        public InvalidFormatException(string message) : base(message) { }

        public InvalidFormatException(string message, Exception inner) : base(message, inner) { }
    }
}