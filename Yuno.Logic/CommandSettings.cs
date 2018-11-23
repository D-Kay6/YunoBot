using System;
using Yuno.Data.Core.Interfaces;
using Yuno.Data.Factory;

namespace Yuno.Logic
{
    [Serializable]
    public class CommandSettings
    {
        private static ISerializer _persistence => SerializerFactory.GenerateSerializer();

        public ulong GuildId { get; private set; }
        public string Prefix { get; private set; }

        public CommandSettings(ulong guildId)
        {
            this.GuildId = guildId;
            this.Prefix = "/";
        }

        public bool ChangePrefix(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return false;
            this.Prefix = value;
            return true;
        }

        public static CommandSettings Load(ulong guildId)
        {
            var data = _persistence.Read<CommandSettings>(guildId);
            if (data != null) return data;
            data = new CommandSettings(guildId);
            return data;
        }

        public void Save()
        {
            _persistence.Write(GuildId, this);
        }
    }
}
