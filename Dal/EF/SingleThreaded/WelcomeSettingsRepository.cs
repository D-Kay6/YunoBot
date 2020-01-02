using Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dal.EF.SingleThreaded
{
    public class WelcomeSettingsRepository : BaseRepository, IDbWelcome
    {
        public async Task<bool> Enable(ulong serverId, ulong channelId)
        {
            var settings = await Context.WelcomeMessages.FindAsync(serverId);
            if (settings == null) return false;
            try
            {
                settings.ChannelId = channelId;
                await Context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> Disable(ulong serverId)
        {
            var settings = await Context.WelcomeMessages.FindAsync(serverId);
            if (settings == null) return false;
            try
            {
                settings.ChannelId = null;
                await Context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> UseImage(ulong serverId, bool value)
        {
            var settings = await Context.WelcomeMessages.FindAsync(serverId);
            if (settings == null) return false;
            try
            {
                settings.UseImage = value;
                await Context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> SetWelcomeMessage(ulong serverId, string message)
        {
            var settings = await Context.WelcomeMessages.FindAsync(serverId);
            if (settings == null) return false;
            try
            {
                settings.Message = message;
                await Context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<WelcomeMessage> GetWelcomeSettings(ulong serverId)
        {
            return await Context.WelcomeMessages.FindAsync(serverId);
        }
    }
}