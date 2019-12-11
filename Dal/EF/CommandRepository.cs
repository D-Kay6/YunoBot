using IDal.Interfaces.Database;
using Microsoft.EntityFrameworkCore;

namespace Dal.EF
{
    public class CommandRepository : ICommand
    {
        private DataContext _context;

        public CommandRepository()
        {
            _context = new DataContext();
        }

        public string GetPrefix(ulong serverId)
        {
            var settings = _context.CommandSettings.Find(serverId);
            return settings?.Prefix;
        }

        public bool SetPrefix(ulong serverId, string prefix)
        {
            var settings = _context.CommandSettings.Find(serverId);
            if (settings == null) return false;
            try
            {
                settings.Prefix = prefix;
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
    }
}
