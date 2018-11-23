using Yuno.Data.Core.Structs;

namespace Yuno.Data.Core.Interfaces
{
    public interface IConfig
    {
        ConfigData Read();
        void Write(ConfigData config);
    }
}
