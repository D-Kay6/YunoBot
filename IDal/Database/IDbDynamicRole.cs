using Core.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IDal.Database
{
    public interface IDbDynamicRole
    {
        Task Add(DynamicRole value);
        Task Update(DynamicRole value);
        Task Remove(DynamicRole value);
        Task<DynamicRole> Get(ulong id);
        Task<List<DynamicRole>> List(ulong serverId);
    }
}