namespace Dal.Database.MySql.EF.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Entity;
    using IDal.Database;
    using IDal.Exceptions;
    using Microsoft.EntityFrameworkCore;

    public class CustomCommandRepository : BaseRepository, IDbCommandCustom
    {
        public async Task Add(CustomCommand value)
        {
            try
            {
                Context.CustomCommands.Add(value);
                await Context.SaveChangesAsync();
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
            Context.CustomCommands.Update(value);
            await Context.SaveChangesAsync();
        }

        public async Task Remove(CustomCommand value)
        {
            try
            {
                Context.CustomCommands.Remove(value);
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                Exception exception = e;
                while (exception.InnerException != null)
                    exception = exception.InnerException;

                if (exception.Message.Contains(
                    "Database operation expected to affect 1 row(s) but actually affected 0 row(s)."))
                    throw new InvalidItemException("the item does not exist in the database.");
            }
        }

        public async Task<CustomCommand> Get(ulong serverId, string command)
        {
            return await Context.CustomCommands
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ServerId == serverId && x.Command == command);
        }

        public async Task<List<CustomCommand>> List(ulong serverId)
        {
            return await Context.CustomCommands
                .AsNoTracking()
                .Where(x => x.ServerId == serverId)
                .ToListAsync();
        }
    }
}