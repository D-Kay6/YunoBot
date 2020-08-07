using Entity.RavenDB;
using System.Threading.Tasks;

namespace IDal.Database.Raven
{
    public interface IDbServer
    {
        Task Add(Server server);
        Task Update(Server server);
        Task Delete(string id);
        Task<Server> Get(ulong id);
    }
}