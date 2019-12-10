using Entity;
using IDal.Interfaces.Database;
using Microsoft.EntityFrameworkCore;

namespace Dal.EF
{
    public class ChannelRepository : IChannel
    {
        private DataContext _context;

        public ChannelRepository()
        {
            _context = new DataContext();
        }


        public bool IsAutoEnabled(ulong serverId)
        {
            var ac = _context.AutoChannels.Find(serverId);
            return ac != null && ac.Enabled;
        }

        public bool SetAutoEnabled(ulong serverId, bool enabled)
        {
            var ac = _context.AutoChannels.Find(serverId);
            if (ac == null) return false;
            try
            {
                ac.Enabled = enabled;
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
            var ac = _context.AutoChannels.Find(serverId);
            return ac?.Prefix;
        }

        public bool SetAutoPrefix(ulong serverId, string prefix)
        {
            var ac = _context.AutoChannels.Find(serverId);
            if (ac == null) return false;
            try
            {
                ac.Prefix = prefix;
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public string GetAutoName(ulong serverId)
        {
            var ac = _context.AutoChannels.Find(serverId);
            return ac?.Name;
        }

        public bool SetAutoName(ulong serverId, string name)
        {
            var ac = _context.AutoChannels.Find(serverId);
            if (ac == null) return false;
            try
            {
                ac.Name = name;
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
            var pc = _context.PermaChannels.Find(serverId);
            if (pc == null) return false;
            return pc.Enabled;
        }

        public bool SetPermaEnabled(ulong serverId, bool enabled)
        {
            var pc = _context.PermaChannels.Find(serverId);
            if (pc == null) return false;
            try
            {
                pc.Enabled = enabled;
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
            var pc = _context.PermaChannels.Find(serverId);
            return pc?.Prefix;
        }

        public bool SetPermaPrefix(ulong serverId, string prefix)
        {
            var pc = _context.PermaChannels.Find(serverId);
            if (pc == null) return false;
            try
            {
                pc.Prefix = prefix;
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public string GetPermaName(ulong serverId)
        {
            var pc = _context.PermaChannels.Find(serverId);
            return pc?.Name;
        }

        public bool SetPermaName(ulong serverId, string name)
        {
            var pc = _context.PermaChannels.Find(serverId);
            if (pc == null) return false;
            try
            {
                pc.Name = name;
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


        public AutoChannel GetAutoChannel(ulong serverId)
        {
            return _context.AutoChannels.Find(serverId);
        }

        public PermaChannel GetPermaChannel(ulong serverId)
        {
            return _context.PermaChannels.Find(serverId);
        }
    }
}
