using Core.Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Repositories
{
    public class RoleRepository : IDbRole
    {
        public async Task<bool> IsAutoEnabled(ulong serverId)
        {
            await using var context = new DataContext();
            var ar = await context.AutoRoles.FindAsync(serverId);
            return ar != null && ar.Enabled;
        }

        public async Task<bool> SetAutoEnabled(ulong serverId, bool enabled)
        {
            await using var context = new DataContext();
            var ar = await context.AutoRoles.FindAsync(serverId);
            if (ar == null) return false;
            try
            {
                ar.Enabled = enabled;
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<string> GetAutoPrefix(ulong serverId)
        {
            await using var context = new DataContext();
            var ar = await context.AutoRoles.FindAsync(serverId);
            return ar?.Prefix;
        }

        public async Task<bool> SetAutoPrefix(ulong serverId, string prefix)
        {
            await using var context = new DataContext();
            var ar = await context.AutoRoles.FindAsync(serverId);
            if (ar == null) return false;
            try
            {
                ar.Prefix = prefix;
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }


        public async Task<bool> IsPermaEnabled(ulong serverId)
        {
            await using var context = new DataContext();
            var pr = await context.PermaRoles.FindAsync(serverId);
            if (pr == null) return false;
            return pr.Enabled;
        }

        public async Task<bool> SetPermaEnabled(ulong serverId, bool enabled)
        {
            await using var context = new DataContext();
            var pr = await context.PermaRoles.FindAsync(serverId);
            if (pr == null) return false;
            try
            {
                pr.Enabled = enabled;
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<string> GetPermaPrefix(ulong serverId)
        {
            await using var context = new DataContext();
            var pr = await context.PermaRoles.FindAsync(serverId);
            return pr?.Prefix;
        }

        public async Task<bool> SetPermaPrefix(ulong serverId, string prefix)
        {
            await using var context = new DataContext();
            var pr = await context.PermaRoles.FindAsync(serverId);
            if (pr == null) return false;
            try
            {
                pr.Prefix = prefix;
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }


        public async Task<bool> IsGeneratedChannel(ulong serverId, ulong channelId)
        {
            await using var context = new DataContext();
            var channel = await context.GeneratedChannels.FindAsync(serverId, channelId);
            return channel != null;
        }

        public async Task<bool> AddGeneratedChannel(ulong serverId, ulong channelId)
        {
            await using var context = new DataContext();
            try
            {
                context.GeneratedChannels.Add(new GeneratedChannel
                {
                    ServerId = serverId,
                    ChannelId = channelId
                });
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> RemoveGeneratedChannel(ulong serverId, ulong channelId)
        {
            await using var context = new DataContext();
            var channel = await context.GeneratedChannels.FindAsync(serverId, channelId);
            if (channel == null) return false;
            try
            {
                context.GeneratedChannels.Remove(channel);
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }


        public async Task<bool> IsIgnoringRoles(ulong serverId, ulong userId)
        {
            await using var context = new DataContext();
            return await context.IgnoredUsers.FindAsync(serverId, userId) != null;
        }

        public async Task<bool> AddIgnoringRoles(ulong serverId, ulong userId)
        {
            await using var context = new DataContext();
            try
            {
                context.IgnoredUsers.Add(new RoleIgnore
                {
                    ServerId = serverId,
                    UserId = userId
                });
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> RemoveIgnoringRoles(ulong serverId, ulong userId)
        {
            await using var context = new DataContext();
            var user = await context.IgnoredUsers.FindAsync(serverId, userId);
            if (user == null) return false;
            try
            {
                context.IgnoredUsers.Remove(user);
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }


        public async Task<AutoRole> GetAutoChannel(ulong serverId)
        {
            await using var context = new DataContext();
            return await context.AutoRoles.FindAsync(serverId);
        }

        public async Task<PermaRole> GetPermaChannel(ulong serverId)
        {
            await using var context = new DataContext();
            return await context.PermaRoles.FindAsync(serverId);
        }
    }
}