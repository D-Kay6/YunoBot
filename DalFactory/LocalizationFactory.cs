using Dal.Json;
using IDal;

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
