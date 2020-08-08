using Core.Entity;
using System.Threading.Tasks;

namespace IDal.Database
{
    public interface IDbDynamicRoleData
    {
        Task Add(DynamicRoleData value);
        Task Update(DynamicRoleData value);
        Task Remove(DynamicRoleData value);
        Task<DynamicRoleData> Get(ulong roleId, ulong dynamicRoleId);
    }
}