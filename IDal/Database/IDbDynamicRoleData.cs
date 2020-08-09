using Core.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IDal.Database
{
    public interface IDbDynamicRoleData
    {
        Task Add(DynamicRoleData value);
        Task Update(DynamicRoleData value);
        Task Remove(DynamicRoleData value);
        Task<DynamicRoleData> Get(ulong dynamicRoleId, ulong roleId);
        Task<List<DynamicRoleData>> Get(ulong roleId);
    }
}