using IDal.Structs.Configuration;

namespace IDal.Interfaces
{
    public interface IConfig
    {
        ConfigData Read();
        void Write(ConfigData config);
    }
}
