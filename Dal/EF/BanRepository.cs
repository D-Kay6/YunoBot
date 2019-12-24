using Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDal.Database;

namespace Dal.EF
{
    public class BanRepository : IDbBan
    {
        private DataContext _context;

        public BanRepository()
        {
            _context = new DataContext();
        }

        public async Task<bool> IsBanned(ulong userId, ulong serverId)
        {
            var ban = await _context.Bans.FindAsync(userId, serverId);
            return ban != null && (ban.EndDate == null || ban.EndDate > DateTime.Now);
        }

        public async Task<bool> AddBan(ulong userId, ulong serverId, DateTime? endDate = null, string reason = null)
        {
            try
            {
                _context.Bans.Add(new Ban
                {
                    UserId = userId,
                    ServerId = serverId,
                    EndDate = endDate,
                    Reason = reason
                });
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> RemoveBan(ulong userId, ulong serverId)
        {
            var ban = await _context.Bans.FindAsync(userId, serverId);
            if (ban == null) return false;
            return await RemoveBan(ban);
        }

        public async Task<bool> RemoveBan(Ban ban)
        {
            try
            {
                _context.Bans.Remove(ban);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<Ban> GetBan(ulong userId, ulong serverId)
        {
            return await _context.Bans.FindAsync(userId, serverId);
        }

        public async Task<List<Ban>> GetBans(bool expiredOnly = true)
        {
            var query = _context.Bans.AsQueryable();

            if (expiredOnly) query = query.Where(x => x.EndDate < DateTime.Now);

            return await query.ToListAsync();
        }
    }
}
