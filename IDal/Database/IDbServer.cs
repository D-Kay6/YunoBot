using Core.Entity;
using System.Threading.Tasks;

namespace IDal.Database
{
    public interface IDbServer
    {
        Task Add(Server value);
        Task Update(Server value);
        Task Remove(Server value);
        Task<Server> Get(ulong serverId);
    }
}