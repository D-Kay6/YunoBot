using Core.Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Repositories
{
    public class PermaRoleRepository : BaseRepository, IDbPermaRole
    {
        public async Task Add(PermaRole value)
        {
            Context.PermaRoles.Add(value);
            await Context.SaveChangesAsync();
        }

        public async Task Update(PermaRole value)
        {
            try
            {
                var entry = Context.Attach(value);
                entry.State = EntityState.Modified;
            }
            catch
            {
                //ignore
            }

            Context.PermaRoles.Update(value);
            await Context.SaveChangesAsync();
        }

        public async Task Remove(PermaRole value)
        {
            Context.PermaRoles.Remove(value);
            await Context.SaveChangesAsync();
        }

        public async Task<PermaRole> Get(ulong serverId)
        {
            var value = await Context.PermaRoles.FindAsync(serverId);
            if (value != null)
                await Context.Entry(value).ReloadAsync();

            return value;
            //return await Context.PermaRoles
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(x => x.ServerId == serverId);
        }
    }
}