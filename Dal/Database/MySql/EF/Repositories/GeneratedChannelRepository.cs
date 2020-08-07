using Core.Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Repositories
{
    public class GeneratedChannelRepository : BaseRepository, IDbGeneratedChannel
    {
        public async Task Add(GeneratedChannel value)
        {
            Context.GeneratedChannels.Add(value);
            await Context.SaveChangesAsync();
        }

        public async Task Update(GeneratedChannel value)
        {
            //try
            //{
            //    var entry = Context.Attach(value);
            //    entry.State = EntityState.Modified;
            //}
            //catch
            //{
            //    //ignore
            //}

            Context.GeneratedChannels.Update(value);
            await Context.SaveChangesAsync();
        }

        public async Task Remove(GeneratedChannel value)
        {
            Context.GeneratedChannels.Remove(value);
            await Context.SaveChangesAsync();
        }

        public async Task<GeneratedChannel> Get(ulong serverId, ulong channelId)
        {
            var value = await Context.GeneratedChannels.FindAsync(serverId, channelId);
            if (value != null)
                await Context.Entry(value).ReloadAsync();

            return value;
            //return await Context.GeneratedChannels
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(x => x.ServerId == serverId && x.ChannelId == channelId);
        }

        public IQueryable<GeneratedChannel> Query(ulong serverId)
        {
            return Context.GeneratedChannels
                .AsNoTracking()
                .AsQueryable()
                .Where(x => x.ServerId == serverId);
        }
    }
}