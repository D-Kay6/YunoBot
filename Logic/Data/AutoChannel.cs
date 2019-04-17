using Discord;
using System;
using System.Collections.Generic;

namespace Logic.Data
{
    [Serializable]
    public class AutoChannel : Configuration<AutoChannel>
    {
        public string AutoPrefix { get; protected set; }
        public string PermaPrefix { get; protected set; }
        protected HashSet<ulong> Channels;

        public AutoChannel(ulong guildId) : base(guildId)
        {
            Enabled = true;
            AutoPrefix = "➕";
            PermaPrefix = "👥";
            Channels = new HashSet<ulong>();
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

        public void SetAutoChannelIcon(string value)
        {
            AutoPrefix = value;
        }

        public void SetPermaChannelIcon(string value)
        {
            PermaPrefix = value;
        }

        public override void Save()
        {
            Save(this);
        }

        protected int AutoChannelIcon;
        protected int PermaChannelIcon;
        protected override void Update()
        {
            if (AutoPrefix == null) AutoPrefix = char.ConvertFromUtf32(AutoChannelIcon);
            if (PermaPrefix == null) PermaPrefix = char.ConvertFromUtf32(PermaChannelIcon);
        }
    }
}