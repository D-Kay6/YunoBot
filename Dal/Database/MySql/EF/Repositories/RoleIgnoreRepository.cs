using Core.Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Repositories
{
    public class RoleIgnoreRepository : BaseRepository, IDbRoleIgnore
    {
        public async Task Add(DynamicRoleIgnore value)
        {
            Context.IgnoredUsers.Add(value);
            await Context.SaveChangesAsync();
        }

        public async Task Update(DynamicRoleIgnore value)
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

        public async Task Remove(DynamicRoleIgnore value)
        {
            Context.IgnoredUsers.Remove(value);
            await Context.SaveChangesAsync();
        }

        public async Task<DynamicRoleIgnore> Get(ulong serverId, ulong userId)
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