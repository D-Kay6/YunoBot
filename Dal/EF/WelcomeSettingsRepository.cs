using Entity;
using IDal.Interfaces.Database;
using Microsoft.EntityFrameworkCore;

namespace Dal.EF
{
    public class WelcomeSettingsRepository : IWelcome
    {
        private DataContext _context;

        public WelcomeSettingsRepository()
        {
            _context = new DataContext();
        }

        public bool Enable(ulong serverId, ulong channelId)
        {
            var settings = _context.WelcomeMessages.Find(serverId);
            if (settings == null) return false;
            try
            {
                settings.ChannelId = channelId;
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public bool Disable(ulong serverId)
        {
            var settings = _context.WelcomeMessages.Find(serverId);
            if (settings == null) return false;
            try
            {
                settings.ChannelId = null;
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public bool UseImage(ulong serverId, bool value)
        {
            var settings = _context.WelcomeMessages.Find(serverId);
            if (settings == null) return false;
            try
            {
                settings.UseImage = value;
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public bool SetWelcomeMessage(ulong serverId, string message)
        {
            var settings = _context.WelcomeMessages.Find(serverId);
            if (settings == null) return false;
            try
            {
                settings.Message = message;
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public WelcomeMessage GetWelcomeSettings(ulong serverId)
        {
            return _context.WelcomeMessages.Find(serverId);
        }
    }
}
