using Entity;
using System;
using System.IO;
using System.Threading.Tasks;
using IDal;

namespace Dal.Json
{
    public class Config : Json, IConfig
    {
        private const string File = "Config.json";
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
