using Core.Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Repositories
{
    public class RoleRepository : BaseRepository, IDbRole
    {
        public async Task Add(Role value)
        {
            Context.Roles.Add(value);
            await Context.SaveChangesAsync();
        }

        public async Task Update(Role value)
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

            Context.Roles.Update(value);
            await Context.SaveChangesAsync();
        }

        public async Task Remove(Role value)
        {
            Context.Roles.Remove(value);
            await Context.SaveChangesAsync();
        }

        public async Task<Role> Get(ulong id)
        {
            return await Context.Roles.FindAsync(id);
        }
    }
}