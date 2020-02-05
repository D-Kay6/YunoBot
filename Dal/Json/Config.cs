using Entity;
using IDal;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Dal.Json
{
    public class Config : Json, IConfig
    {
#if DEBUG
        private const string File = "Config.Debug.json";
#else
        private const string File = "Config.json";
#endif
        private string Directory => Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents", "Yuno Bot", "Configuration");

        public async Task<Configuration> Read()
        {
            return await ReadAsync<Configuration>(Directory, File);
        }

        public async Task Write(Configuration config)
        {
            await WriteAsync(config, Directory, File);
        }
    }
}