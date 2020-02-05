using Entity.RavenDB;
using IDal.Database.Raven;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System.Threading.Tasks;

namespace Dal.Database.RavenDB.Repositories
{
    public class RoleRepository : BaseRepository, IDbRole
    {
        public async Task Add(RoleAutomation value)
        {
            using var session = Context.GetAsyncSession();
            await session.StoreAsync(value);
            await session.SaveChangesAsync();
        }

        public async Task Update(RoleAutomation value)
        {
            using var session = Context.GetAsyncSession();
            var settings = await session.LoadAsync<Models.RoleAutomation>(value.Id);
            //settings.AutoRole = value.AutoRole;
            //settings.PermaRole = value.PermaRole;
            //settings.IgnoredUsers = value.IgnoredUsers;
            await session.SaveChangesAsync();
        }

        public async Task Remove(string id)
        {
            using var session = Context.GetAsyncSession();
            session.Delete(id);
            await session.SaveChangesAsync();
        }

        public async Task<RoleAutomation> Get(ulong serverId)
        {
            using var session = Context.GetAsyncSession();
            var server = await session.Query<Server>()
                //.Where(x => x.ServerId == serverId)
                .Include(x => x.RoleAutomation)
                .FirstOrDefaultAsync();
            if (server == null) return null;
            return await session.LoadAsync<RoleAutomation>(server.RoleAutomation);
        }
    }
}