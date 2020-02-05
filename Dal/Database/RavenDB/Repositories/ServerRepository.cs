using Dal.Database.RavenDB.Indexes;
using Entity.RavenDB;
using IDal.Database.Raven;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System.Threading.Tasks;

namespace Dal.Database.RavenDB.Repositories
{
    public class ServerRepository : BaseRepository, IDbServer
    {
        public async Task Add(ulong id, string name)
        {
            using var session = Context.GetAsyncSession();

            var channelSettings = new ChannelAutomation();
            await session.StoreAsync(channelSettings);

            var roleSettings = new RoleAutomation();
            await session.StoreAsync(roleSettings);

            var server = new Server
            {
                //ServerId = id,
                Name = name,
                ChannelAutomation = channelSettings.Id,
                RoleAutomation = roleSettings.Id
            };
            await session.StoreAsync(server);

            await session.SaveChangesAsync();
        }

        public async Task Add(Server value)
        {
            using var session = Context.GetAsyncSession();
            var server = new Models.Server
            {
                ServerId = value.ServerId.ToString(),
                Name = value.Name,
                ChannelAutomation = value.ChannelAutomation,
                RoleAutomation = value.RoleAutomation,
                LanguageSetting = value.LanguageSetting,
                //CommandSetting = value.CommandSetting,
                //WelcomeMessage = value.WelcomeMessage
            };
            await session.StoreAsync(server);
            await session.SaveChangesAsync();
        }

        public async Task Update(ulong id, string name)
        {
            using var session = Context.GetAsyncSession();
            var server = await session.Query<Server, Servers_ById>()
                //.Where(x => x.ServerId == id)
                .FirstOrDefaultAsync();
            server.Name = name;
            await session.SaveChangesAsync();
        }

        public async Task Update(Server value)
        {
            using var session = Context.GetAsyncSession();
            await session.SaveChangesAsync();
        }

        public async Task Delete(string id)
        {
            using var session = Context.GetAsyncSession();
            var server = await session.Query<Models.Server, Servers_ById>()
                .Where(x => x.ServerId == id)
                .FirstOrDefaultAsync();
            session.Delete(server.Id);
            await session.SaveChangesAsync();
        }

        public async Task<Server> Get(ulong id)
        {
            using var session = Context.GetAsyncSession();
            return await session.Query<Server>()
                .Where(x => x.ServerId == id)
                .FirstOrDefaultAsync();
        }

        public async Task Save()
        {
            using var session = Context.GetAsyncSession();
            await session.SaveChangesAsync();
        }
    }
}