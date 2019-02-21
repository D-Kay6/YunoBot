using Discord;
using System;
using System.Collections.Generic;

namespace Logic.Data
{
    [Serializable]
    public class AutoChannel : Configuration<AutoChannel>
    {
        protected int AutoChannelIcon;
        protected HashSet<ulong> Channels;
        protected int PermaChannelIcon;

        public AutoChannel(ulong guildId) : base(guildId)
        {
            Enabled = true;
            Channels = new HashSet<ulong>();
            AutoChannelIcon = 10133;
            PermaChannelIcon = 128101;
        }

        public bool Enabled { get; }

        public void LoadChannels(IEnumerable<ulong> channels)
        {
            Channels.UnionWith(channels);
        }

        public bool AddChannel(ulong id)
        {
            return Channels.Add(id);
        }

        public bool RemoveChannel(ulong id)
        {
            return Channels.Remove(id);
        }

        public bool IsControlledChannel(ulong id)
        {
            return Channels.Contains(id);
        }

        public bool IsAutoChannel(IChannel channel)
        {
            var id = char.ConvertToUtf32(channel.Name, 0);
            return AutoChannelIcon.Equals(id);
        }

        public bool IsPermaChannel(IChannel channel)
        {
            var id = char.ConvertToUtf32(channel.Name, 0);
            return PermaChannelIcon.Equals(id);
        }

        public string GetAutoChannelIcon()
        {
            return char.ConvertFromUtf32(AutoChannelIcon);
        }

        public string GetPermaChannelIcon()
        {
            return char.ConvertFromUtf32(PermaChannelIcon);
        }

        public void SetAutoChannelIcon(int value)
        {
            AutoChannelIcon = value;
        }

        public void SetAutoChannelIcon(string value)
        {
            AutoChannelIcon = char.ConvertToUtf32(value, 0);
        }

        public void SetPermaChannelIcon(int value)
        {
            PermaChannelIcon = value;
        }

        public void SetPermaChannelIcon(string value)
        {
            PermaChannelIcon = char.ConvertToUtf32(value, 0);
        }

        protected override void Update()
        {
            if (PermaChannelIcon == 0) PermaChannelIcon = 128101;
        }

        public override void Save()
        {
            Save(this);
        }
    }
}