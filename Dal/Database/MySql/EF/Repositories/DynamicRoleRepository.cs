using Core.Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Repositories
{
    public class DynamicRoleRepository : BaseRepository, IDbDynamicRole
    {
        public Task Add(DynamicRole value)
        {
            Context.DynamicRoles.Add(value);
            return Context.SaveChangesAsync();
        }

        public Task Update(DynamicRole value)
        {
            Context.DynamicRoles.Update(value);
            return Context.SaveChangesAsync();
        }

        public Task Remove(DynamicRole value)
        {
            Context.DynamicRoles.Remove(value);
            return Context.SaveChangesAsync();
        }

        public Task Remove(IEnumerable<DynamicRole> values)
        {
            foreach (var value in values)
            {
                Context.DynamicRoles.Remove(value);
            }
            return Context.SaveChangesAsync();
        }

        public async Task<DynamicRole> Get(ulong id)
        {
            return await Context.DynamicRoles.FindAsync(id);
        }

        public Task<List<DynamicRole>> List(ulong serverId)
        {
            return Context.DynamicRoles
                .Include(x => x.Roles)
                .Where(x => x.ServerId == serverId)
                .ToListAsync();
        }
    }
}