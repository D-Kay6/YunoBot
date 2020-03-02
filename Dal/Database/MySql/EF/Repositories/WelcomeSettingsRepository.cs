using Core.Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Repositories
{
    public class WelcomeSettingsRepository : IDbWelcome
    {
        public async Task<bool> Enable(ulong serverId, ulong channelId)
        {
            await using var context = new DataContext();
            var settings = await context.WelcomeMessages.FindAsync(serverId);
            if (settings == null) return false;
            try
            {
                settings.ChannelId = channelId;
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> Disable(ulong serverId)
        {
            await using var context = new DataContext();
            var settings = await context.WelcomeMessages.FindAsync(serverId);
            if (settings == null) return false;
            try
            {
                settings.ChannelId = null;
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> UseImage(ulong serverId, bool value)
        {
            await using var context = new DataContext();
            var settings = await context.WelcomeMessages.FindAsync(serverId);
            if (settings == null) return false;
            try
            {
                settings.UseImage = value;
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> SetWelcomeMessage(ulong serverId, string message)
        {
            await using var context = new DataContext();
            var settings = await context.WelcomeMessages.FindAsync(serverId);
            if (settings == null) return false;
            try
            {
                settings.Message = message;
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<WelcomeMessage> GetWelcomeSettings(ulong serverId)
        {
            await using var context = new DataContext();
            return await context.WelcomeMessages.FindAsync(serverId);
        }
    }
}