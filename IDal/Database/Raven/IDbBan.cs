using Entity.RavenDB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IDal.Database.Raven
{
    public interface IDbBan
    {
        Task AddBan(Ban value, Server server);
        Task UpdateBan(Ban value);
        Task RemoveBan(string id);
        Task<Ban> GetBan(ulong userId, ulong serverId);
        Task<List<Ban>> GetBans(bool expiredOnly = true);
    }
}