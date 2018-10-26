using Yuno.Data.Core.Interfaces;
using Yuno.Data.Json;

namespace Yuno.Data.Factory
{
    public class ConfigFactory
    {
        public static IConfig GenerateConfig()
        {
            return new Configuration();
        }
    }
}
