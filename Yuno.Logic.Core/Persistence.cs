using System;
using System.Collections.Generic;

namespace Yuno.Logic.Core
{
    [Serializable]
    public class Persistence
    {
        public AutoChannel AutoChannel { get; protected set; }

        public Persistence()
        {
            this.AutoChannel = new AutoChannel();
        }
    }
}