using Entity.RavenDB;
using IDal.Database.Raven;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System.Threading.Tasks;

namespace Dal.Database.RavenDB.Repositories
{
    public class ChannelRepository : BaseRepository, IDbChannel
    {
        public async Task Add(ChannelAutomation value)
        {
            using var session = Context.GetAsyncSession();
            await session.StoreAsync(value);
            await session.SaveChangesAsync();
        }

        public async Task Update(ChannelAutomation value)
        {
            using var session = Context.GetAsyncSession();
            var settings = await session.LoadAsync<Models.ChannelAutomation>(value.Id);
            //settings.AutoChannel = value.AutoChannel;
            //settings.PermaChannel = value.PermaChannel;
            await session.SaveChangesAsync();
        }

        public async Task Remove(string id)
        {
            using var session = Context.GetAsyncSession();
            session.Delete(id);
            await session.SaveChangesAsync();
        }

        public async Task<ChannelAutomation> Get(ulong serverId)
        {
            using var session = Context.GetAsyncSession();
            var server = await session.Query<Server>()
                //.Where(x => x.ServerId == serverId)
                .Include(x => x.ChannelAutomation)
                .FirstOrDefaultAsync();
            if (server == null) return null;
            return await session.LoadAsync<ChannelAutomation>(server.ChannelAutomation);
        }
    }
}