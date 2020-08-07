using Entity.RavenDB;
using System.Threading.Tasks;

namespace IDal.Database.Raven
{
    public interface IDbRole
    {
        Task Add(RoleAutomation value);
        Task Update(RoleAutomation value);
        Task Remove(string id);
        Task<RoleAutomation> Get(ulong serverId);
    }
}