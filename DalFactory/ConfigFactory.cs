namespace DalFactory
{
    using Dal.Json;
    using IDal;

    public class ConfigFactory
    {
        public static IConfig GenerateConfig()
        {
            return new Config();
        }
    }
}