using IDal;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Dal.Json
{
    public class Localization : Json, ILocalization
    {
        private string Directory => Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents",
            "Yuno Bot", "Localization");

        public async Task<Core.Entity.Localization> Read(string language)
        {
            return await ReadAsync<Core.Entity.Localization>(Directory, $"{language}.json");
        }
    }
}