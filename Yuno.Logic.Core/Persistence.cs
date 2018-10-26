using System;
using System.Collections.Generic;

namespace Yuno.Logic.Core
{
    [Serializable]
    public class Persistence
    {
        public CommandSettings CommandSettings { get; protected set; }
        public AutoChannel AutoChannel { get; protected set; }

        public Persistence()
        {
            this.CommandSettings = new CommandSettings();
            this.AutoChannel = new AutoChannel();
        }
    }
}