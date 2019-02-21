using IDal.Interfaces;
using IDal.Structs.Configuration;

namespace Dal.Json
{
    public class Configuration : Json, IConfig
    {
        private const string Directory = "Configuration";

        private const string File = "Config.json";

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
