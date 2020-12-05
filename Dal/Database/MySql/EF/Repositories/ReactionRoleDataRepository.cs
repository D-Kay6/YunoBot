using Core.Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Repositories
{
    public class ReactionRoleDataRepository : BaseRepository, IDbReactionRoleData
    {
        public Task Add(ReactionRoleData value)
        {
            Context.ReactionRoleData.Add(value);
            return Context.SaveChangesAsync();
        }

        public Task Update(ReactionRoleData value)
        {
            Context.ReactionRoleData.Update(value);
            return Context.SaveChangesAsync();
        }

        public Task Remove(ReactionRoleData value)
        {
            Context.ReactionRoleData.Remove(value);
            return Context.SaveChangesAsync();
        }

        public async Task<ReactionRoleData> Get(ulong reactionRoleId, ulong roleId)
        {
            return await Context.ReactionRoleData.FindAsync(reactionRoleId, roleId);
        }
    }
}