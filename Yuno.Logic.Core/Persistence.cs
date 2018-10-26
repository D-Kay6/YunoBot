using System;
using System.Collections.Generic;

namespace Yuno.Logic.Core
{
    [Serializable]
    public class Persistence
    {
        public AutoChannel AutoChannel { get; protected set; }

        public Persistence(IEnumerable<ulong> channels = null, int autoChannelIcon = 0)
        {
            AutoChannel = new AutoChannel(channels, autoChannelIcon);
        }
    }
}