using System;
using System.IO;
using IDal.Interfaces;
using IDal.Structs.Configuration;

namespace Dal.Json
{
    public class Configuration : Json, IConfig
    {
        private const string File = "Config.json";
        private string Directory => Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents", "Yuno Bot", "Configuration");

        public ConfigData Read()
        {
            return Read<ConfigData>(Directory, File);
        }

        public void Write(ConfigData config)
        {
            Write(config, Directory, File);
        }
    }
}
