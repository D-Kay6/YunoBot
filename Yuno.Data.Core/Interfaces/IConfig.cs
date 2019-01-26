using Yuno.Data.Core.Structs;
using Yuno.Data.Core.Structs.Configuration;

namespace Yuno.Data.Core.Interfaces
{
    public interface IConfig
    {
        ConfigData Read();
        void Write(ConfigData config);
    }
}
