using Core.Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Repositories
{
    public class AutoChannelRepository : BaseRepository, IDbAutoChannel
    {
        public async Task Add(AutoChannel value)
        {
            Context.AutoChannels.Add(value);
            await Context.SaveChangesAsync();
        }

        public async Task Update(AutoChannel value)
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

            Context.AutoChannels.Update(value);
            await Context.SaveChangesAsync();
        }

        public async Task Remove(AutoChannel value)
        {
            Context.AutoChannels.Remove(value);
            await Context.SaveChangesAsync();
        }

        public async Task<AutoChannel> Get(ulong serverId)
        {
            var value = await Context.AutoChannels.FindAsync(serverId);
            if (value != null)
                await Context.Entry(value).ReloadAsync();

            return value;
            //return await Context.AutoChannels
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(x => x.ServerId == serverId);
        }
    }
}