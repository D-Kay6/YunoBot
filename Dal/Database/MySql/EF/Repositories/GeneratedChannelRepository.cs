namespace Dal.Database.MySql.EF.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Entity;
    using IDal.Database;
    using Microsoft.EntityFrameworkCore;

    public class GeneratedChannelRepository : BaseRepository, IDbGeneratedChannel
    {
        public async Task Add(GeneratedChannel value)
        {
            Context.GeneratedChannels.Add(value);
            await Context.SaveChangesAsync();
        }

        public async Task Update(GeneratedChannel value)
        {
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
            return await Context.GeneratedChannels
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ServerId == serverId && x.ChannelId == channelId);
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