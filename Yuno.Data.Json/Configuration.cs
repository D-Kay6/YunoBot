using Yuno.Data.Core.Interfaces;
using Yuno.Data.Core.Structs;

namespace Yuno.Data.Json
{
    public class Configuration : Yuno.Data.Json.Json, IConfig
    {
        private const string Directory = "Configuration";
        private const string File = "Config.json";

        public Config Read()
        {
            return Read<Config>(Directory, File);
        }

        public void Write(Config config)
        {
            Write(config, Directory, File);
        }
    }
}
