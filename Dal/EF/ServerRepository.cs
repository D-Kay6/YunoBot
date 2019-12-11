using Entity;
using IDal.Interfaces.Database;
using Microsoft.EntityFrameworkCore;

namespace Dal.EF
{
    public class ServerRepository : IServer
    {
        private DataContext _context;

        public ServerRepository()
        {
            _context = new DataContext();
        }

        public void AddServer(ulong id, string name)
        {
            _context.Servers.Add(new Server
            {
                Id = id,
                Name = name
            });
            _context.SaveChanges();
        }

        public void AddServer(Server server)
        {
            try
            {
                _context.Servers.Add(server);
                _context.SaveChanges();
            }
            catch (DbUpdateException)
            {

            }
        }

        public void UpdateServer(ulong id, string name)
        {
            var server = _context.Servers.Find(id);
            if (server == null)
            {
                AddServer(id, name);
                return;
            }

            server.Name = name;
            _context.SaveChanges();
        }

        public void UpdateServer(Server server)
        {
            _context.Servers.Update(server);
            _context.SaveChanges();
        }

        public void DeleteServer(ulong id)
        {
            var server = _context.Servers.Find(id);
            if (server == null) return;
            _context.Servers.Remove(server);
            _context.SaveChanges();
        }

        public Server GetServer(ulong id)
        {
            return _context.Servers.Find(id);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
