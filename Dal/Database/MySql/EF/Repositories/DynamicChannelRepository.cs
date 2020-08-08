using Core.Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Repositories
{
    public class DynamicChannelRepository : BaseRepository, IDbDynamicChannel
    {
        public Task Add(DynamicChannel value)
        {
            Context.DynamicChannels.Add(value);
            return Context.SaveChangesAsync();
        }

        public Task Update(DynamicChannel value)
        {
            Context.DynamicChannels.Update(value);
            return Context.SaveChangesAsync();
        }

        public Task Remove(DynamicChannel value)
        {
            Context.DynamicChannels.Remove(value);
            return Context.SaveChangesAsync();
        }

        public async Task<DynamicChannel> Get(ulong id)
        {
            return await Context.DynamicChannels.FindAsync(id);
        }

        public Task<List<DynamicChannel>> List(ulong serverId)
        {
            return Context.DynamicChannels.AsNoTracking()
                .Where(x => x.ServerId == serverId)
                .ToListAsync();
        }
    }
}