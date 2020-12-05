using Dal.Json;
using IDal;

namespace DalFactory
{
    public class ConfigFactory
    {
        public static IConfig GenerateConfig()
        {
            return new Config();
        }
    }
}