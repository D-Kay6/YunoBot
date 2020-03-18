namespace DalFactory
{
    using Dal.Json;
    using IDal;

    public class LocalizationFactory
    {
        public static ILocalization GenerateLocalization()
        {
            return new Localization();
        }
    }
}