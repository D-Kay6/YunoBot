using Yuno.Data.Core.Structs;
using Yuno.Data.Core.Structs.Data;

namespace Yuno.Data.Core.Interfaces
{
    public interface ISerializer
    {
        T Read<T>(ulong id);
        void Write<T>(ulong id, T data);
    }
}
