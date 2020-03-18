namespace Dal.Database.MySql.EF.Repositories
{
    using System.Threading.Tasks;
    using Core.Entity;
    using IDal.Database;
    using Microsoft.EntityFrameworkCore;

    public class PermaChannelRepository : BaseRepository, IDbPermaChannel
    {
        public async Task Add(PermaChannel value)
        {
            Context.PermaChannels.Add(value);
            await Context.SaveChangesAsync();
        }

        public async Task Update(PermaChannel value)
        {
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
            return await Context.PermaChannels
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ServerId == serverId);
        }
    }
}