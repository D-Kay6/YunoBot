using IDal.Interfaces;
using IDal.Structs;

namespace Dal.Json.Settings
{
    public class ConfigDal : ConfigDefaults, IConfigDal
    {
        public ConfigDalData Load()
        {
            return this.Read<ConfigDalData>(Directory, File);
        }
    }
}
