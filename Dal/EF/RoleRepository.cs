using Entity;
using IDal.Interfaces.Database;
using Microsoft.EntityFrameworkCore;

namespace Dal.EF
{
    public class RoleRepository : IRole
    {
        private DataContext _context;

        public RoleRepository()
        {
            _context = new DataContext();
        }


        public bool IsAutoEnabled(ulong serverId)
        {
            var ar = _context.AutoRoles.Find(serverId);
            return ar != null && ar.Enabled;
        }

        public bool SetAutoEnabled(ulong serverId, bool enabled)
        {
            var ar = _context.AutoRoles.Find(serverId);
            if (ar == null) return false;
            try
            {
                ar.Enabled = enabled;
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public string GetAutoPrefix(ulong serverId)
        {
            var ar = _context.AutoRoles.Find(serverId);
            return ar?.Prefix;
        }

        public bool SetAutoPrefix(ulong serverId, string prefix)
        {
            var ar = _context.AutoRoles.Find(serverId);
            if (ar == null) return false;
            try
            {
                ar.Prefix = prefix;
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }


        public bool IsPermaEnabled(ulong serverId)
        {
            var pr = _context.PermaRoles.Find(serverId);
            if (pr == null) return false;
            return pr.Enabled;
        }

        public bool SetPermaEnabled(ulong serverId, bool enabled)
        {
            var pr = _context.PermaRoles.Find(serverId);
            if (pr == null) return false;
            try
            {
                pr.Enabled = enabled;
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public string GetPermaPrefix(ulong serverId)
        {
            var pr = _context.PermaRoles.Find(serverId);
            return pr?.Prefix;
        }

        public bool SetPermaPrefix(ulong serverId, string prefix)
        {
            var pr = _context.PermaRoles.Find(serverId);
            if (pr == null) return false;
            try
            {
                pr.Prefix = prefix;
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }


        public bool IsGeneratedChannel(ulong serverId, ulong channelId)
        {
            var channel = _context.GeneratedChannels.Find(serverId, channelId);
            return channel != null;
        }

        public bool AddGeneratedChannel(ulong serverId, ulong channelId)
        {
            try
            {
                _context.GeneratedChannels.Add(new GeneratedChannel
                {
                    ServerId = serverId,
                    ChannelId = channelId
                });
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public bool RemoveGeneratedChannel(ulong serverId, ulong channelId)
        {
            var channel = _context.GeneratedChannels.Find(serverId, channelId);
            if (channel == null) return false;
            try
            {
                _context.GeneratedChannels.Remove(channel);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }


        public bool IsIgnoringRoles(ulong serverId, ulong userId)
        {
            return _context.IgnoredUsers.Find(serverId, userId) != null;
        }

        public bool AddIgnoringRoles(ulong serverId, ulong userId)
        {
            try
            {
                _context.IgnoredUsers.Add(new RoleIgnore
                {
                    ServerId = serverId,
                    UserId = userId
                });
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public bool RemoveIgnoringRoles(ulong serverId, ulong userId)
        {
            var user = _context.IgnoredUsers.Find(serverId, userId);
            if (user == null) return false;
            try
            {
                _context.IgnoredUsers.Remove(user);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }


        public AutoRole GetAutoChannel(ulong serverId)
        {
            return _context.AutoRoles.Find(serverId);
        }

        public PermaRole GetPermaChannel(ulong serverId)
        {
            return _context.PermaRoles.Find(serverId);
        }
    }
}
