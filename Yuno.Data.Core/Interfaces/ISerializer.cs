using Yuno.Data.Core.Structs;

namespace Yuno.Data.Core.Interfaces
{
    public interface ISerializer
    {
        Persistence Read(ulong id);
        void Write(ulong id, Persistence data);
    }
}
