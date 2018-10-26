using System;
using System.Collections.Generic;

namespace Yuno.Data.Core.Structs
{
    [Serializable]
    public struct Persistence
    {
        public HashSet<ulong> Channels;
        public int AutoChannelIcon;

        public Persistence(IEnumerable<ulong> channels = null, int autoChannelIcon = 0)
        {
            this.Channels = new HashSet<ulong>(channels);
            this.AutoChannelIcon = autoChannelIcon;
        }
    }
}
