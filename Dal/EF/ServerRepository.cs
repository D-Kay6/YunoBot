using System;
using System.Threading.Tasks;
using Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;

namespace Dal.EF
{
    public class ServerRepository : IDbServer
    {
        private DataContext _context;

        public ServerRepository()
        {
            _context = new DataContext();
        }

        public async Task AddServer(ulong id, string name)
        {
            _context.Servers.Add(new Server
            {
                Id = id,
                Name = name
            });
            await _context.SaveChangesAsync();
        }

        public async Task AddServer(Server server)
        {
            try
            {
                _context.Servers.Add(server);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {

            }
        }

        public async Task UpdateServer(ulong id, string name)
        {
            try
            {
                var server = await _context.Servers.FindAsync(id);
                if (server == null)
                {
                    await AddServer(id, name);
                    return;
                }

                server.Name = name;
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {

            }
        }

        public async Task UpdateServer(Server server)
        {
            _context.Servers.Update(server);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteServer(ulong id)
        {
            var server = await _context.Servers.FindAsync(id);
            if (server == null) return;
            _context.Servers.Remove(server);
            await _context.SaveChangesAsync();
        }

        public async Task<Server> GetServer(ulong id)
        {
            return await _context.Servers.FindAsync(id);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}
