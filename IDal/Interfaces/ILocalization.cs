using IDal.Structs.Localization;

namespace IDal.Interfaces
{
    public interface ILocalization
    {
        LanguageData Read(string language);
    }
}