using System;
using System.Collections.Generic;
using Yuno.Logic.Core;

namespace Yuno.Logic
{
    [Serializable]
    public class AutoChannelLogic : AutoChannel
    {
        public void LoadChannels(IEnumerable<ulong> channels)
        {
            _channels.UnionWith(channels);
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

        public void SetAutoChannelIcon(int value)
        {
            AutoChannelIcon = value;
        }

        public void SetAutoChannelIcon(string value)
        {
            AutoChannelIcon = char.ConvertToUtf32(value, 0);
        }
    }
}
