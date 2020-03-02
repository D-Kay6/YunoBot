using Core.Entity;
using IDal.Database;
using IDal.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Repositories
{
    public class CustomCommandRepository : IDbCommandCustom
    {
        public async Task Add(CustomCommand value)
        {
            await using var context = new DataContext();
            try
            {
                context.CustomCommands.Add(value);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                Exception exception = e;
                while (exception.InnerException != null) 
                    exception = exception.InnerException;

                if (exception.Message.Contains("Duplicate entry"))
                    throw new ItemExistsException("the item already exists in the database.");
            }
        }

        public async Task Update(CustomCommand value)
        {
            await using var context = new DataContext();
            context.CustomCommands.Update(value);
            await context.SaveChangesAsync();
        }

        public async Task Remove(CustomCommand value)
        {
            await using var context = new DataContext();
            try
            {
                context.CustomCommands.Remove(value);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                Exception exception = e;
                while (exception.InnerException != null) 
                    exception = exception.InnerException;

                if (exception.Message.Contains("Database operation expected to affect 1 row(s) but actually affected 0 row(s)."))
                    throw new InvalidItemException("the item does not exist in the database.");
            }
        }

        public async Task<CustomCommand> Get(ulong serverId, string command)
        {
            await using var context = new DataContext();
            return await context.CustomCommands.FindAsync(serverId, command);
        }

        public async Task<List<CustomCommand>> List(ulong serverId)
        {
            await using var context = new DataContext();
            return await context.CustomCommands
                .Where(x => x.ServerId == serverId)
                .ToListAsync();
        }
    }
}