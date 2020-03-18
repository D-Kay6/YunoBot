namespace Dal.Database.MySql.EF.Repositories
{
    using System.Threading.Tasks;
    using Core.Entity;
    using IDal.Database;
    using Microsoft.EntityFrameworkCore;

    public class AutoChannelRepository : BaseRepository, IDbAutoChannel
    {
        public async Task Add(AutoChannel value)
        {
            Context.AutoChannels.Add(value);
            await Context.SaveChangesAsync();
        }

        public async Task Update(AutoChannel value)
        {
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
            return await Context.AutoChannels
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ServerId == serverId);
        }
    }
}