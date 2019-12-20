using System.Collections.Generic;
using System.Threading.Tasks;
using Entity;
using IDal.Interfaces.Database;
using Microsoft.EntityFrameworkCore;

namespace Dal.EF
{
    public class CommandRepository : IDbCommand
    {
        private DataContext _context;

        public CommandRepository()
        {
            _context = new DataContext();
        }

        public async Task<string> GetPrefix(ulong serverId)
        {
            var settings = await _context.CommandSettings.FindAsync(serverId);
            return settings?.Prefix;
        }

        public async Task<bool> SetPrefix(ulong serverId, string prefix)
        {
            var settings = await _context.CommandSettings.FindAsync(serverId);
            if (settings == null) return false;
            try
            {
                settings.Prefix = prefix;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> AddCustomCommand(ulong serverId, string command, string response)
        {
            var customCommand = new CustomCommand
            {
                ServerId = serverId,
                Command = command,
                Response = response
            };
            return await AddCustomCommand(customCommand);
        }

        public async Task<bool> AddCustomCommand(CustomCommand customCommand)
        {
            try
            {
                _context.CustomCommands.Add(customCommand);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> RemoveCustomCommand(ulong serverId, string command)
        {
            var customCommand = await GetCustomCommand(serverId, command);
            return await RemoveCustomCommand(customCommand);
        }

        public async Task<bool> RemoveCustomCommand(CustomCommand customCommand)
        {
            if (customCommand == null) return false;
            try
            {
                _context.CustomCommands.Remove(customCommand);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<CustomCommand> GetCustomCommand(ulong serverId, string command)
        {
            return await _context.CustomCommands.FindAsync(serverId, command);
        }

        public async Task<List<CustomCommand>> GetCustomCommands(ulong serverId)
        {
            return await _context.CustomCommands.ToListAsync();
        }
    }
}
