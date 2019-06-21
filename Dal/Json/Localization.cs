using IDal.Interfaces;
using IDal.Structs.Localization;

namespace Dal.Json
{
    public class Localization : Json, ILocalization
    {
        private const string Directory = "Localization";

        public LanguageData Read(string language)
        {
            return Read<LanguageData>(Directory, $"{language}.json");
        }
    }
}
