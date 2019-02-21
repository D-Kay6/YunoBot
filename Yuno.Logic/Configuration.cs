using System;
using Yuno.Data.Core.Interfaces;
using Yuno.Data.Factory;

namespace Yuno.Logic
{
    [Serializable]
    public abstract class Configuration<T> where T : Configuration<T>
    {
        private static ISerializer Persistence => SerializerFactory.GenerateSerializer();

        public ulong GuildId { get; }

        protected Configuration(ulong guildId)
        {
            GuildId = guildId;
        }

        public static T Load(ulong guildId)
        {
            var data = Persistence.Read<T>(guildId);
            if (data != null)
            {
                data.Update();
                return data;
            }
            data = (T)Activator.CreateInstance(typeof(T), guildId);
            data.Save();
            return data;
        }

        public static void Remove(ulong guildId)
        {
            var data = (T)Activator.CreateInstance(typeof(T), guildId);
            data.Save();
        }

        protected void Save(T data)
        {
            Persistence.Write(GuildId, data);
        }

        public abstract void Save();

        protected abstract void Update();
    }
}