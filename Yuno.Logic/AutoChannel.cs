using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Yuno.Data.Core.Interfaces;
using Yuno.Data.Core.Structs;
using Yuno.Data.Factory;

namespace Yuno.Logic
{
    [Serializable]
    public class AutoChannel
    {
        private static ISerializer _persistence => SerializerFactory.GenerateSerializer();

        public ulong GuildId { get; private set; }
        public bool Enabled { get; private set; }
        protected int AutoChannelIcon;
        protected HashSet<ulong> Channels;

        public AutoChannel(ulong guildId)
        {
            this.GuildId = guildId;
            this.Enabled = true;
            this.Channels = new HashSet<ulong>();
            this.AutoChannelIcon = 10133;
        }

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

        public bool IsAutoChannel(string value)
        {
            var id = char.ConvertToUtf32(value, 0);
            return AutoChannelIcon.Equals(id);
        }

        public string GetAutoChannelIcon()
        {
            return char.ConvertFromUtf32(AutoChannelIcon);
        }

        public void SetAutoChannelIcon(int value)
        {
            AutoChannelIcon = value;
        }

        public void SetAutoChannelIcon(string value)
        {
            AutoChannelIcon = char.ConvertToUtf32(value, 0);
        }

        public static AutoChannel Load(ulong guildId)
        {
            var data = _persistence.Read<AutoChannel>(guildId);
            if (data != null) return data;
            data = new AutoChannel(guildId);
            data.Save();
            return data;
        }

        public void Save()
        {
            _persistence.Write(GuildId, this);
        }
    }
}
