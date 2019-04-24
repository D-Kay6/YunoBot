using Discord;
using System;
using System.Collections.Generic;

namespace Logic.Data
{
    [Serializable]
    public class AutoChannel : Configuration<AutoChannel>
    {
        public string AutoPrefix { get; protected set; }
        public string AutoName { get; protected set; }
        public string PermaPrefix { get; protected set; }
        public string PermaName { get; protected set; }
        protected HashSet<ulong> Channels;

        public AutoChannel(ulong guildId) : base(guildId)
        {
            Enabled = true;
            AutoPrefix = "➕";
            AutoName = "--channel";
            PermaPrefix = "👥";
            PermaName = "{0} channel";
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
            return channel.Name.StartsWith(AutoPrefix, StringComparison.Ordinal);
        }

        public bool IsPermaChannel(IChannel channel)
        {
            return channel.Name.StartsWith(PermaPrefix, StringComparison.Ordinal);
        }

        public bool SetAutoChannelPrefix(string value)
        {
            if (PermaPrefix == value) return false;
            AutoPrefix = value;
            return true;
        }

        public void SetAutoChannelName(string value)
        {
            AutoName = value;
        }

        public bool SetPermaChannelPrefix(string value)
        {
            if (AutoPrefix == value) return false;
            PermaPrefix = value;
            return true;
        }

        public void SetPermaChannelName(string value)
        {
            PermaName = value;
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
            if (AutoName == null)
            {
                AutoName = "--channel";
                PermaName = "{0} channel";
            }
        }
    }
}