using Core.Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Repositories
{
    public class PermaChannelRepository : BaseRepository, IDbPermaChannel
    {
        public async Task Add(PermaChannel value)
        {
            Context.PermaChannels.Add(value);
            await Context.SaveChangesAsync();
        }

        public async Task Update(PermaChannel value)
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

            Context.PermaChannels.Update(value);
            await Context.SaveChangesAsync();
        }

        public async Task Remove(PermaChannel value)
        {
            Context.PermaChannels.Remove(value);
            await Context.SaveChangesAsync();
        }

        public async Task<PermaChannel> Get(ulong serverId)
        {
            var value = await Context.PermaChannels.FindAsync(serverId);
            if (value != null)
                await Context.Entry(value).ReloadAsync();

            return value;
            //return await Context.PermaChannels
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(x => x.ServerId == serverId);
        }
    }
}