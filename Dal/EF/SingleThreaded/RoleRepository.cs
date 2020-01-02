using Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dal.EF.SingleThreaded
{
    public class RoleRepository : BaseRepository, IDbRole
    {
        public async Task<bool> IsAutoEnabled(ulong serverId)
        {
            var ar = await Context.AutoRoles.FindAsync(serverId);
            return ar != null && ar.Enabled;
        }

        public async Task<bool> SetAutoEnabled(ulong serverId, bool enabled)
        {
            var ar = await Context.AutoRoles.FindAsync(serverId);
            if (ar == null) return false;
            try
            {
                ar.Enabled = enabled;
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
            var ar = await Context.AutoRoles.FindAsync(serverId);
            return ar?.Prefix;
        }

        public async Task<bool> SetAutoPrefix(ulong serverId, string prefix)
        {
            var ar = await Context.AutoRoles.FindAsync(serverId);
            if (ar == null) return false;
            try
            {
                ar.Prefix = prefix;
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
            var pr = await Context.PermaRoles.FindAsync(serverId);
            if (pr == null) return false;
            return pr.Enabled;
        }

        public async Task<bool> SetPermaEnabled(ulong serverId, bool enabled)
        {
            var pr = await Context.PermaRoles.FindAsync(serverId);
            if (pr == null) return false;
            try
            {
                pr.Enabled = enabled;
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
            var pr = await Context.PermaRoles.FindAsync(serverId);
            return pr?.Prefix;
        }

        public async Task<bool> SetPermaPrefix(ulong serverId, string prefix)
        {
            var pr = await Context.PermaRoles.FindAsync(serverId);
            if (pr == null) return false;
            try
            {
                pr.Prefix = prefix;
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


        public async Task<bool> IsIgnoringRoles(ulong serverId, ulong userId)
        {
            return await Context.IgnoredUsers.FindAsync(serverId, userId) != null;
        }

        public async Task<bool> AddIgnoringRoles(ulong serverId, ulong userId)
        {
            try
            {
                Context.IgnoredUsers.Add(new RoleIgnore
                {
                    ServerId = serverId,
                    UserId = userId
                });
                await Context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> RemoveIgnoringRoles(ulong serverId, ulong userId)
        {
            var user = await Context.IgnoredUsers.FindAsync(serverId, userId);
            if (user == null) return false;
            try
            {
                Context.IgnoredUsers.Remove(user);
                await Context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }


        public async Task<AutoRole> GetAutoChannel(ulong serverId)
        {
            return await Context.AutoRoles.FindAsync(serverId);
        }

        public async Task<PermaRole> GetPermaChannel(ulong serverId)
        {
            return await Context.PermaRoles.FindAsync(serverId);
        }
    }
}