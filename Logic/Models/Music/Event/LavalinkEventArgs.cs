﻿using System;

namespace Logic.Models.Music.Event
{
    public class LavalinkEventArgs : EventArgs
    {
        public string Message { get; private set; }

        public LavalinkEventArgs(string message)
        {
            Message = message;
        }
    }
}