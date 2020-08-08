using Core.Entity;
using System.Threading.Tasks;

namespace IDal.Database
{
    public interface IDbRoleIgnore
    {
        Task Add(DynamicRoleIgnore value);
        Task Update(DynamicRoleIgnore value);
        Task Remove(DynamicRoleIgnore value);
        Task<DynamicRoleIgnore> Get(ulong serverId, ulong userId);
    }
}