using Core.Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Repositories
{
    public class ReactionRoleRepository : BaseRepository, IDbReactionRole
    {
        public Task Add(ReactionRole value)
        {
            Context.ReactionRoles.Add(value);
            return Context.SaveChangesAsync();
        }

        public Task Update(ReactionRole value)
        {
            Context.ReactionRoles.Update(value);
            return Context.SaveChangesAsync();
        }

        public Task Remove(ReactionRole value)
        {
            Context.ReactionRoles.Remove(value);
            return Context.SaveChangesAsync();
        }

        public async Task<ReactionRole> Get(ulong id)
        {
            return await Context.ReactionRoles.FindAsync(id);
        }

        public Task<List<ReactionRole>> List(ulong serverId)
        {
            return Context.ReactionRoles.AsNoTracking()
                .Where(x => x.ServerId == serverId)
                .ToListAsync();
        }
    }
}