using System;
using System.Collections.Generic;

namespace Yuno.Data.Core.Structs
{
    [Serializable]
    public struct AutoChannelData
    {
        public bool Enabled { get; private set; }
        public int AutoChannelIcon { get; private set; }
        public HashSet<ulong> Channels { get; private set; }

        public AutoChannelData(bool enabled = true, int autoChannelIcon = 0, IEnumerable<ulong> channels = null)
        {
            this.Enabled = enabled;
            this.AutoChannelIcon = autoChannelIcon;
            this.Channels = new HashSet<ulong>(channels ?? new List<ulong>());
        }
    }
}
