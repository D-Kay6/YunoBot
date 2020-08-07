using Core.Entity;
using System.Threading.Tasks;

namespace IDal.Database
{
    public interface IDbRoleIgnore
    {
        Task Add(RoleIgnore value);
        Task Update(RoleIgnore value);
        Task Remove(RoleIgnore value);
        Task<RoleIgnore> Get(ulong serverId, ulong userId);
    }
}