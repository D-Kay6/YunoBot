using Core.Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Repositories
{
    public class ServerRepository : BaseRepository, IDbServer
    {
        public async Task Add(Server value)
        {
            Context.Servers.Add(value);
            await Context.SaveChangesAsync();
        }

        public async Task Update(Server value)
        {
            try
            {
                var entry = Context.Attach(value);
                entry.State = EntityState.Modified;
            }
            catch
            {
                //ignore
            }

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