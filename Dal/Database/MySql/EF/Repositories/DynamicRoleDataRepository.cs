using Core.Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Repositories
{
    public class DynamicRoleDataRepository : BaseRepository, IDbDynamicRoleData
    {
        public Task Add(DynamicRoleData value)
        {
            Context.DynamicRoleData.Add(value);
            return Context.SaveChangesAsync();
        }

        public Task Update(DynamicRoleData value)
        {
            Context.DynamicRoleData.Update(value);
            return Context.SaveChangesAsync();
        }

        public async Task Remove(DynamicRoleData value)
        {
            var roleData = Context.Entry(value);
            if (roleData == null) return;

            Context.DynamicRoleData.Remove(roleData.Entity);
            await Context.SaveChangesAsync();
        }

        public async Task<DynamicRoleData> Get(ulong dynamicRoleId, ulong roleId)
        {
            return await Context.DynamicRoleData.FindAsync(dynamicRoleId, roleId);
        }

        public Task<List<DynamicRoleData>> Get(ulong roleId)
        {
            return Context.DynamicRoleData.AsNoTracking()
                .Include(x => x.DynamicRole)
                .Where(x => x.RoleId == roleId)
                .ToListAsync();
        }
    }
}