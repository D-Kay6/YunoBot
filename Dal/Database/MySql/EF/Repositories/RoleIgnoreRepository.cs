using Core.Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Repositories
{
    public class RoleIgnoreRepository : BaseRepository, IDbRoleIgnore
    {
        public async Task Add(RoleIgnore value)
        {
            Context.IgnoredUsers.Add(value);
            await Context.SaveChangesAsync();
        }

        public async Task Update(RoleIgnore value)
        {
            //try
            //{
            //    var entry = Context.Attach(value);
            //    entry.State = EntityState.Modified;
            //}
            //catch
            //{
            //    //ignore
            //}

            Context.IgnoredUsers.Update(value);
            await Context.SaveChangesAsync();
        }

        public async Task Remove(RoleIgnore value)
        {
            Context.IgnoredUsers.Remove(value);
            await Context.SaveChangesAsync();
        }

        public async Task<RoleIgnore> Get(ulong serverId, ulong userId)
        {
            var value = await Context.IgnoredUsers.FindAsync(serverId, userId);
            if (value != null)
                await Context.Entry(value).ReloadAsync();

            return value;
            //return await Context.IgnoredUsers
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(x => x.ServerId == serverId && x.UserId == userId);
        }
    }
}