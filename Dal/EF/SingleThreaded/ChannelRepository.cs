using Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dal.EF.SingleThreaded
{
    public class ChannelRepository : BaseRepository, IDbChannel
    {
        public async Task<bool> IsAutoEnabled(ulong serverId)
        {
            var ac = await Context.AutoChannels.FindAsync(serverId);
            return ac != null && ac.Enabled;
        }

        public async Task<bool> SetAutoEnabled(ulong serverId, bool enabled)
        {
            var ac = await Context.AutoChannels.FindAsync(serverId);
            if (ac == null) return false;
            try
            {
                ac.Enabled = enabled;
                await Context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<string> GetAutoPrefix(ulong serverId)
        {
            var ac = await Context.AutoChannels.FindAsync(serverId);
            return ac?.Prefix;
        }

        public async Task<bool> SetAutoPrefix(ulong serverId, string prefix)
        {
            var ac = await Context.AutoChannels.FindAsync(serverId);
            if (ac == null) return false;
            try
            {
                ac.Prefix = prefix;
                await Context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<string> GetAutoName(ulong serverId)
        {
            var ac = await Context.AutoChannels.FindAsync(serverId);
            return ac?.Name;
        }

        public async Task<bool> SetAutoName(ulong serverId, string name)
        {
            var ac = await Context.AutoChannels.FindAsync(serverId);
            if (ac == null) return false;
            try
            {
                ac.Name = name;
                await Context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }


        public async Task<bool> IsPermaEnabled(ulong serverId)
        {
            var pc = await Context.PermaChannels.FindAsync(serverId);
            if (pc == null) return false;
            return pc.Enabled;
        }

        public async Task<bool> SetPermaEnabled(ulong serverId, bool enabled)
        {
            var pc = await Context.PermaChannels.FindAsync(serverId);
            if (pc == null) return false;
            try
            {
                pc.Enabled = enabled;
                await Context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<string> GetPermaPrefix(ulong serverId)
        {
            var pc = await Context.PermaChannels.FindAsync(serverId);
            return pc?.Prefix;
        }

        public async Task<bool> SetPermaPrefix(ulong serverId, string prefix)
        {
            var pc = await Context.PermaChannels.FindAsync(serverId);
            if (pc == null) return false;
            try
            {
                pc.Prefix = prefix;
                await Context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<string> GetPermaName(ulong serverId)
        {
            var pc = await Context.PermaChannels.FindAsync(serverId);
            return pc?.Name;
        }

        public async Task<bool> SetPermaName(ulong serverId, string name)
        {
            var pc = await Context.PermaChannels.FindAsync(serverId);
            if (pc == null) return false;
            try
            {
                pc.Name = name;
                await Context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }


        public async Task<bool> IsGeneratedChannel(ulong serverId, ulong channelId)
        {
            var channel = await Context.GeneratedChannels.FindAsync(serverId, channelId);
            return channel != null;
        }

        public async Task<bool> AddGeneratedChannel(ulong serverId, ulong channelId)
        {
            try
            {
                Context.GeneratedChannels.Add(new GeneratedChannel
                {
                    ServerId = serverId,
                    ChannelId = channelId
                });
                await Context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> RemoveGeneratedChannel(ulong serverId, ulong channelId)
        {
            var channel = await Context.GeneratedChannels.FindAsync(serverId, channelId);
            if (channel == null) return false;
            try
            {
                Context.GeneratedChannels.Remove(channel);
                await Context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }


        public async Task<AutoChannel> GetAutoChannel(ulong serverId)
        {
            return await Context.AutoChannels.FindAsync(serverId);
        }

        public async Task<PermaChannel> GetPermaChannel(ulong serverId)
        {
            return await Context.PermaChannels.FindAsync(serverId);
        }
    }
}