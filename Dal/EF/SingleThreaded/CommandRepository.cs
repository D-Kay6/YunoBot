using Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.EF.SingleThreaded
{
    public class CommandRepository : BaseRepository, IDbCommand
    {
        public async Task<string> GetPrefix(ulong serverId)
        {
            var settings = await Context.CommandSettings.FindAsync(serverId);
            return settings?.Prefix;
        }

        public async Task<bool> SetPrefix(ulong serverId, string prefix)
        {
            var settings = await Context.CommandSettings.FindAsync(serverId);
            if (settings == null) return false;
            try
            {
                settings.Prefix = prefix;
                await Context.SaveChangesAsync();
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
                Context.CustomCommands.Add(customCommand);
                await Context.SaveChangesAsync();
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
                Context.CustomCommands.Remove(customCommand);
                await Context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<CustomCommand> GetCustomCommand(ulong serverId, string command)
        {
            return await Context.CustomCommands.FindAsync(serverId, command);
        }

        public async Task<List<CustomCommand>> GetCustomCommands(ulong serverId)
        {
            return await Context.CustomCommands.Where(x => x.ServerId == serverId).ToListAsync();
        }
    }
}