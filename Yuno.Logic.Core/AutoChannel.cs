using System;
using System.Collections.Generic;
using System.Linq;

namespace Yuno.Logic.Core
{
    [Serializable]
    public class AutoChannel
    {
        protected HashSet<ulong> _channels;

        public IReadOnlyCollection<ulong> Channels
        {
            get { return _channels; }
        }
        public int AutoChannelIcon { get; protected set; }

        public AutoChannel()
        {
            this._channels = new HashSet<ulong>();
            this.AutoChannelIcon = 10133;
        }
        
        public AutoChannel(IEnumerable<ulong> channels = null, int autoChannelIcon = 10133)
        {
            this._channels = channels == null ? new HashSet<ulong>() : new HashSet<ulong>(channels);
            this.AutoChannelIcon = autoChannelIcon;
        }
    }
}