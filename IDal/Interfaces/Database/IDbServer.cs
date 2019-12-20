using System.Threading.Tasks;
using Entity;

namespace IDal.Interfaces.Database
{
    public interface IDbServer
    {
        Task AddServer(ulong id, string name);
        Task AddServer(Server server);
        Task UpdateServer(ulong id, string name);
        Task UpdateServer(Server server);
        Task DeleteServer(ulong id);
        Task<Server> GetServer(ulong id);
        Task Save();
    }
}
