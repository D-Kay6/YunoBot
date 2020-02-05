using System;
using System.Threading.Tasks;
using Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;

namespace Dal.Database.MySql.EF.SingleThreaded
{
    public class ServerRepository : BaseRepository, IDbServer
    {
        public async Task AddServer(ulong id, string name)
        {
            Context.Servers.Add(new Server
            {
                Id = id,
                Name = name
            });
            await Context.SaveChangesAsync();
        }

        public async Task AddServer(Server server)
        {
            try
            {
                Context.Servers.Add(server);
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {

            }
        }

        public async Task UpdateServer(ulong id, string name)
        {
            try
            {
                var server = await Context.Servers.FindAsync(id);
                if (server == null)
                {
                    await AddServer(id, name);
                    return;
                }

                server.Name = name;
                await Context.SaveChangesAsync();
            }
            catch (Exception e)
            {

            }
        }

        public async Task UpdateServer(Server server)
        {
            Context.Servers.Update(server);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteServer(ulong id)
        {
            var server = await Context.Servers.FindAsync(id);
            if (server == null) return;
            Context.Servers.Remove(server);
            await Context.SaveChangesAsync();
        }

        public async Task<Server> GetServer(ulong id)
        {
            return await Context.Servers.FindAsync(id);
        }

        public async Task Save()
        {
            await Context.SaveChangesAsync();
        }
    }
}