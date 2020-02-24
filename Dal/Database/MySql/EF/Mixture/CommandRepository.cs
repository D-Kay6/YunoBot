﻿using Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Mixture
{
    public class CommandRepository : IDbCommand
    {
        public async Task<string> GetPrefix(ulong serverId)
        {
            await using var context = new DataContext();
            var settings = await context.CommandSettings.FindAsync(serverId);
            return settings?.Prefix;
        }

        public async Task<bool> SetPrefix(ulong serverId, string prefix)
        {
            await using var context = new DataContext();
            var settings = await context.CommandSettings.FindAsync(serverId);
            if (settings == null) return false;
            try
            {
                settings.Prefix = prefix;
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> AddCustomCommand(ulong serverId, string command, string response)
        {
            await using var context = new DataContext();
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
            await using var context = new DataContext();
            try
            {
                context.CustomCommands.Add(customCommand);
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<bool> RemoveCustomCommand(ulong serverId, string command)
        {
            await using var context = new DataContext();
            var customCommand = await GetCustomCommand(serverId, command);
            return await RemoveCustomCommand(customCommand);
        }

        public async Task<bool> RemoveCustomCommand(CustomCommand customCommand)
        {
            if (customCommand == null) return false;
            await using var context = new DataContext();
            try
            {
                context.CustomCommands.Remove(customCommand);
                await context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public async Task<CustomCommand> GetCustomCommand(ulong serverId, string command)
        {
            await using var context = new DataContext();
            return await context.CustomCommands.FindAsync(serverId, command);
        }

        public async Task<List<CustomCommand>> GetCustomCommands(ulong serverId)
        {
            await using var context = new DataContext();
            return await context.CustomCommands.Where(x => x.ServerId == serverId).ToListAsync();
        }
    }
}