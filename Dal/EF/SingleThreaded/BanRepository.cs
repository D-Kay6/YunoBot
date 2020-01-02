using Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.EF.SingleThreaded
{
    public class BanRepository : BaseRepository, IDbBan
    {
        public async Task<bool> IsBanned(ulong userId, ulong serverId)
        {
            var ban = await Context.Bans.FindAsync(userId, serverId);
            return ban != null && (ban.EndDate == null || ban.EndDate > DateTime.Now);
        }

        public async Task<bool> AddBan(ulong userId, ulong serverId, DateTime? endDate = null, string reason = null)
        {
            try
            {
                Context.Bans.Add(new Ban
                {
                    UserId = userId,
                    ServerId = serverId,
                    EndDate = endDate,
                    Reason = reason
                });
                await Context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> RemoveBan(ulong userId, ulong serverId)
        {
            var ban = await Context.Bans.FindAsync(userId, serverId);
            if (ban == null) return false;
            return await RemoveBan(ban);
        }

        public async Task<bool> RemoveBan(Ban ban)
        {
            try
            {
                Context.Bans.Remove(ban);
                await Context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<Ban> GetBan(ulong userId, ulong serverId)
        {
            return await Context.Bans.FindAsync(userId, serverId);
        }

        public async Task<List<Ban>> GetBans(bool expiredOnly = true)
        {
            var query = Context.Bans.AsQueryable();

            if (expiredOnly) query = query.Where(x => x.EndDate < DateTime.Now);

            return await query.ToListAsync();
        }
    }
}