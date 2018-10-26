using Yuno.Data.Core.Structs;

namespace Yuno.Data.Core.Interfaces
{
    public interface IConfig
    {
        Config Read();
        void Write(Config config);
    }
}
