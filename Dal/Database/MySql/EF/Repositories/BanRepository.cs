namespace Dal.Database.MySql.EF.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Entity;
    using IDal.Database;
    using Microsoft.EntityFrameworkCore;

    public class BanRepository : BaseRepository, IDbBan
    {
        public async Task Add(Ban value)
        {
            Context.Bans.Add(value);
            await Context.SaveChangesAsync();
        }

        public async Task Update(Ban value)
        {
            Context.Bans.Update(value);
            await Context.SaveChangesAsync();
        }

        public async Task Remove(Ban value)
        {
            Context.Bans.Remove(value);
            await Context.SaveChangesAsync();
        }

        public async Task<Ban> Get(ulong userId, ulong serverId)
        {
            return await Context.Bans.FindAsync(userId, serverId);
        }

        public async Task<List<Ban>> List(ulong? serverId = null, bool expiredOnly = true)
        {
            var query = Context.Bans.AsQueryable();

            if (serverId.HasValue) query = query.Where(x => x.ServerId == serverId.Value);
            if (expiredOnly) query = query.Where(x => x.EndDate < DateTime.Now);

            return await query.ToListAsync();
        }
    }
}