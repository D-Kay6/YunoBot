using System.Threading.Tasks;
using Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;

namespace Dal.EF
{
    public class RoleRepository : IDbRole
    {
        private DataContext _context;

        public RoleRepository()
        {
            _context = new DataContext();
        }


        public async Task<bool> IsAutoEnabled(ulong serverId)
        {
            var ar = await _context.AutoRoles.FindAsync(serverId);
            return ar != null && ar.Enabled;
        }

        public async Task<bool> SetAutoEnabled(ulong serverId, bool enabled)
        {
            var ar = await _context.AutoRoles.FindAsync(serverId);
            if (ar == null) return false;
            try
            {
                ar.Enabled = enabled;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<string> GetAutoPrefix(ulong serverId)
        {
            var ar = await _context.AutoRoles.FindAsync(serverId);
            return ar?.Prefix;
        }

        public async Task<bool> SetAutoPrefix(ulong serverId, string prefix)
        {
            var ar = await _context.AutoRoles.FindAsync(serverId);
            if (ar == null) return false;
            try
            {
                ar.Prefix = prefix;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }


        public async Task<bool> IsPermaEnabled(ulong serverId)
        {
            var pr = await _context.PermaRoles.FindAsync(serverId);
            if (pr == null) return false;
            return pr.Enabled;
        }

        public async Task<bool> SetPermaEnabled(ulong serverId, bool enabled)
        {
            var pr = await _context.PermaRoles.FindAsync(serverId);
            if (pr == null) return false;
            try
            {
                pr.Enabled = enabled;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<string> GetPermaPrefix(ulong serverId)
        {
            var pr = await _context.PermaRoles.FindAsync(serverId);
            return pr?.Prefix;
        }

        public async Task<bool> SetPermaPrefix(ulong serverId, string prefix)
        {
            var pr = await _context.PermaRoles.FindAsync(serverId);
            if (pr == null) return false;
            try
            {
                pr.Prefix = prefix;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }


        public async Task<bool> IsGeneratedChannel(ulong serverId, ulong channelId)
        {
            var channel = await _context.GeneratedChannels.FindAsync(serverId, channelId);
            return channel != null;
        }

        public async Task<bool> AddGeneratedChannel(ulong serverId, ulong channelId)
        {
            try
            {
                _context.GeneratedChannels.Add(new GeneratedChannel
                {
                    ServerId = serverId,
                    ChannelId = channelId
                });
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> RemoveGeneratedChannel(ulong serverId, ulong channelId)
        {
            var channel = await _context.GeneratedChannels.FindAsync(serverId, channelId);
            if (channel == null) return false;
            try
            {
                _context.GeneratedChannels.Remove(channel);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }


        public async Task<bool> IsIgnoringRoles(ulong serverId, ulong userId)
        {
            return await _context.IgnoredUsers.FindAsync(serverId, userId) != null;
        }

        public async Task<bool> AddIgnoringRoles(ulong serverId, ulong userId)
        {
            try
            {
                _context.IgnoredUsers.Add(new RoleIgnore
                {
                    ServerId = serverId,
                    UserId = userId
                });
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> RemoveIgnoringRoles(ulong serverId, ulong userId)
        {
            var user = await _context.IgnoredUsers.FindAsync(serverId, userId);
            if (user == null) return false;
            try
            {
                _context.IgnoredUsers.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }


        public async Task<AutoRole> GetAutoChannel(ulong serverId)
        {
            return await _context.AutoRoles.FindAsync(serverId);
        }

        public async Task<PermaRole> GetPermaChannel(ulong serverId)
        {
            return await _context.PermaRoles.FindAsync(serverId);
        }
    }
}
