namespace Dal.Database.MySql.EF.Repositories
{
    using System.Threading.Tasks;
    using Core.Entity;
    using IDal.Database;

    public class ServerRepository : BaseRepository, IDbServer
    {
        public async Task Add(Server value)
        {
            Context.Servers.Add(value);
            await Context.SaveChangesAsync();
        }

        public async Task Update(Server value)
        {
            Context.Servers.Update(value);
            await Context.SaveChangesAsync();
        }

        public async Task Remove(Server value)
        {
            Context.Servers.Remove(value);
            await Context.SaveChangesAsync();
        }

        public async Task<Server> Get(ulong serverId)
        {
            return await Context.Servers.FindAsync(serverId);
        }
    }
}