using System.Collections.Generic;
using System.Linq;
using Yuno.Data.Core.Structs;

namespace Yuno.Main.AutoChannel
{
    class AutoChannel
    {
        private HashSet<ulong> _channels;

        public IReadOnlyCollection<ulong> Channels
        {
            get { return _channels; }
        }
        public int AutoChannelIcon { get; private set; }
        
        public AutoChannel()
        {
            this._channels = new HashSet<ulong>();
            this.AutoChannelIcon = 10133;
        }

        public bool AddChannel(ulong id)
        {
            return _channels.Add(id);
        }

        public bool RemoveChannel(ulong id)
        {
            return _channels.Remove(id);
        }

        public bool IsControlledChannel(ulong id)
        {
            return _channels.Contains(id);
        }

        public bool IsAutoChannel(string value)
        {
            var id = char.ConvertToUtf32(value, 0);
            return AutoChannelIcon.Equals(id);
        }

        public bool IsGeneratedChannel(ulong id)
        {
            return _channels.Contains(id);
        }

        public void SetAutoChannelIcon(string value)
        {
            AutoChannelIcon = char.ConvertToUtf32(value, 0);
        }

        public void Load(Persistence data)
        {
            if (data.AutoChannelIcon != 0) this.AutoChannelIcon = data.AutoChannelIcon;
            if (data.Channels != null && data.Channels.Any()) this._channels = new HashSet<ulong>(data.Channels);
        }
    }
}
