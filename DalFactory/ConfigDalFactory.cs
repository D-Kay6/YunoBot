using Dal.Json.Settings;
using IDal.Interfaces;

namespace DalFactory
{
    public static class ConfigDalFactory
    {
        public static IConfigDal GenerateConfigDal()
        {
            return new ConfigDal();
        }
    }
}
