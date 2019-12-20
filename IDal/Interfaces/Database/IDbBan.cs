using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Entity;

namespace IDal.Interfaces.Database
{
    public interface IDbBan
    {
        Task<bool> IsBanned(ulong userId, ulong serverId);
        Task<bool> AddBan(ulong userId, ulong serverId, DateTime? endDate = null, string reason = null);
        Task<bool> RemoveBan(ulong userId, ulong serverId);
        Task<bool> RemoveBan(Ban ban);
        Task<Ban> GetBan(ulong userId, ulong serverId);
        Task<List<Ban>> GetBans(bool expiredOnly = true);
    }
}