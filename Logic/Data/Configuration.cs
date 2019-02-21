using System;
using DalFactory;
using IDal.Interfaces;

namespace Logic.Data
{
    [Serializable]
    public abstract class Configuration<T> where T : Configuration<T>
    {
        protected Configuration(ulong guildId)
        {
            GuildId = guildId;
        }

        private static ISerializer Persistence => SerializerFactory.GenerateSerializer();

        public ulong GuildId { get; }

        public static T Load(ulong guildId)
        {
            var data = Persistence.Read<T>(guildId);
            if (data != null)
            {
                data.Update();
                return data;
            }

            data = (T) Activator.CreateInstance(typeof(T), guildId);
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