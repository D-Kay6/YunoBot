using Core.Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Repositories
{
    public class AutoRoleRepository : BaseRepository, IDbAutoRole
    {
        public async Task Add(AutoRole value)
        {
            Context.AutoRoles.Add(value);
            await Context.SaveChangesAsync();
        }

        public async Task Update(AutoRole value)
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

            Context.AutoRoles.Update(value);
            await Context.SaveChangesAsync();
        }

        public async Task Remove(AutoRole value)
        {
            Context.AutoRoles.Remove(value);
            await Context.SaveChangesAsync();
        }

        public async Task<AutoRole> Get(ulong serverId)
        {
            var value = await Context.AutoRoles.FindAsync(serverId);
            if (value != null)
                await Context.Entry(value).ReloadAsync();

            return value;
            //return await Context.AutoRoles
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(x => x.ServerId == serverId);
        }
    }
}