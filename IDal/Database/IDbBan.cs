using Core.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IDal.Database
{
    public interface IDbBan
    {
        Task Add(Ban value);
        Task Update(Ban value);
        Task Remove(Ban value);
        Task<Ban> Get(ulong userId, ulong serverId);
        Task<List<Ban>> List(ulong? serverId = null, bool expiredOnly = true);
    }
}