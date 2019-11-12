using System;
using System.IO;
using IDal.Interfaces;
using IDal.Structs.Localization;

namespace Dal.Json
{
    public class Localization : Json, ILocalization
    {
        private string Directory => Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents", "Yuno Bot", "Localization");

        public LanguageData Read(string language)
        {
            return Read<LanguageData>(Directory, $"{language}.json");
        }
    }
}
