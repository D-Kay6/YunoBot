using Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.EF
{
    public class BanRepository : IDbBan
    {
        public async Task<bool> IsBanned(ulong userId, ulong serverId)
        {
            var context = new DataContext();
            var ban = await context.Bans.FindAsync(userId, serverId);
            return ban != null && (ban.EndDate == null || ban.EndDate > DateTime.Now);
        }

        public async Task<bool> AddBan(ulong userId, ulong serverId, DateTime? endDate = null, string reason = null)
        {
            var context = new DataContext();
            try
            {
                context.Bans.Add(new Ban
                {
                    UserId = userId,
                    ServerId = serverId,
                    EndDate = endDate,
                    Reason = reason
                });
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> RemoveBan(ulong userId, ulong serverId)
        {
            var context = new DataContext();
            var ban = await context.Bans.FindAsync(userId, serverId);
            if (ban == null) return false;
            return await RemoveBan(ban);
        }

        public async Task<bool> RemoveBan(Ban ban)
        {
            var context = new DataContext();
            try
            {
                context.Bans.Remove(ban);
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<Ban> GetBan(ulong userId, ulong serverId)
        {
            var context = new DataContext();
            return await context.Bans.FindAsync(userId, serverId);
        }

        public async Task<List<Ban>> GetBans(bool expiredOnly = true)
        {
            var context = new DataContext();
            var query = context.Bans.AsQueryable();
            if (expiredOnly) query = query.Where(x => x.EndDate < DateTime.Now);
            return await query.ToListAsync();
        }
    }
}
