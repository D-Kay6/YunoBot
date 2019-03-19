using Dal.Json;
using IDal.Interfaces;

namespace DalFactory
{
    public class ConfigFactory
    {
        public static IConfig GenerateConfig()
        {
            return new Configuration();
        }
    }
}
