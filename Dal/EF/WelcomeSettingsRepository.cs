using System.Threading.Tasks;
using Entity;
using IDal.Interfaces.Database;
using Microsoft.EntityFrameworkCore;

namespace Dal.EF
{
    public class WelcomeSettingsRepository : IDbWelcome
    {
        private DataContext _context;

        public WelcomeSettingsRepository()
        {
            _context = new DataContext();
        }

        public async Task<bool> Enable(ulong serverId, ulong channelId)
        {
            var settings = await _context.WelcomeMessages.FindAsync(serverId);
            if (settings == null) return false;
            try
            {
                settings.ChannelId = channelId;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> Disable(ulong serverId)
        {
            var settings = await _context.WelcomeMessages.FindAsync(serverId);
            if (settings == null) return false;
            try
            {
                settings.ChannelId = null;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> UseImage(ulong serverId, bool value)
        {
            var settings = await _context.WelcomeMessages.FindAsync(serverId);
            if (settings == null) return false;
            try
            {
                settings.UseImage = value;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> SetWelcomeMessage(ulong serverId, string message)
        {
            var settings = await _context.WelcomeMessages.FindAsync(serverId);
            if (settings == null) return false;
            try
            {
                settings.Message = message;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<WelcomeMessage> GetWelcomeSettings(ulong serverId)
        {
            return await _context.WelcomeMessages.FindAsync(serverId);
        }
    }
}
