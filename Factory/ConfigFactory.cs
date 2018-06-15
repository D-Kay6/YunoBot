using Dal.Json.Settings;
using ILogic.Interfaces;
using Logic.Configuration.Settings;

namespace Factory
{
    public static class ConfigFactory
    {
        public static IConfig GenerateConfig()
        {
            return new Config(new ConfigDal());
        }
    }
}
