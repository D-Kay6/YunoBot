using Dal.Json;
using IDal.Interfaces;

namespace DalFactory
{
    public class LocalizationFactory
    {
        public static ILocalization GenerateLocalization()
        {
            return new Localization();
        }
    }
}
