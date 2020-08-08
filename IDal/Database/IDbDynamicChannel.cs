using Core.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IDal.Database
{
    public interface IDbDynamicChannel
    {
        Task Add(DynamicChannel value);
        Task Update(DynamicChannel value);
        Task Remove(DynamicChannel value);
        Task<DynamicChannel> Get(ulong id);
        Task<List<DynamicChannel>> List(ulong serverId);
    }
}