using System;

namespace Logic.Models.Music.Event
{
    public class PlayerExceptionEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public PlayerExceptionEventArgs(string message)
        {
            Message = message;
        }
    }
}