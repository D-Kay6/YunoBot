using Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Mixture
{
    public class ChannelRepository : IDbChannel
    {
        public async Task<bool> IsAutoEnabled(ulong serverId)
        {
            await using var context = new DataContext();
            var ac = await context.AutoChannels.FindAsync(serverId);
            return ac != null && ac.Enabled;
        }

        public async Task<bool> SetAutoEnabled(ulong serverId, bool enabled)
        {
            await using var context = new DataContext();
            var ac = await context.AutoChannels.FindAsync(serverId);
            if (ac == null) return false;
            try
            {
                ac.Enabled = enabled;
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
            var ac = await context.AutoChannels.FindAsync(serverId);
            return ac?.Prefix;
        }

        public async Task<bool> SetAutoPrefix(ulong serverId, string prefix)
        {
            await using var context = new DataContext();
            var ac = await context.AutoChannels.FindAsync(serverId);
            if (ac == null) return false;
            try
            {
                ac.Prefix = prefix;
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<string> GetAutoName(ulong serverId)
        {
            await using var context = new DataContext();
            var ac = await context.AutoChannels.FindAsync(serverId);
            return ac?.Name;
        }

        public async Task<bool> SetAutoName(ulong serverId, string name)
        {
            await using var context = new DataContext();
            var ac = await context.AutoChannels.FindAsync(serverId);
            if (ac == null) return false;
            try
            {
                ac.Name = name;
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
            var pc = await context.PermaChannels.FindAsync(serverId);
            if (pc == null) return false;
            return pc.Enabled;
        }

        public async Task<bool> SetPermaEnabled(ulong serverId, bool enabled)
        {
            await using var context = new DataContext();
            var pc = await context.PermaChannels.FindAsync(serverId);
            if (pc == null) return false;
            try
            {
                pc.Enabled = enabled;
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
            var pc = await context.PermaChannels.FindAsync(serverId);
            return pc?.Prefix;
        }

        public async Task<bool> SetPermaPrefix(ulong serverId, string prefix)
        {
            await using var context = new DataContext();
            var pc = await context.PermaChannels.FindAsync(serverId);
            if (pc == null) return false;
            try
            {
                pc.Prefix = prefix;
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<string> GetPermaName(ulong serverId)
        {
            await using var context = new DataContext();
            var pc = await context.PermaChannels.FindAsync(serverId);
            return pc?.Name;
        }

        public async Task<bool> SetPermaName(ulong serverId, string name)
        {
            await using var context = new DataContext();
            var pc = await context.PermaChannels.FindAsync(serverId);
            if (pc == null) return false;
            try
            {
                pc.Name = name;
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


        public async Task<AutoChannel> GetAutoChannel(ulong serverId)
        {
            await using var context = new DataContext();
            return await context.AutoChannels.FindAsync(serverId);
        }

        public async Task<PermaChannel> GetPermaChannel(ulong serverId)
        {
            await using var context = new DataContext();
            return await context.PermaChannels.FindAsync(serverId);
        }
    }
}