using Yuno.Logic.Core;

namespace Yuno.Data.Core.Interfaces
{
    public interface ISerializer
    {
        Persistence Read(ulong id);
        void Write(ulong id, Persistence data);
    }
}
