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

        public Task Remove(DynamicRoleData value)
        {
            Context.DynamicRoleData.Remove(value);
            return Context.SaveChangesAsync();
        }

        public async Task<DynamicRoleData> Get(ulong roleId, ulong dynamicRoleId)
        {
            return await Context.DynamicRoleData.FindAsync(roleId, dynamicRoleId);
        }
    }
}