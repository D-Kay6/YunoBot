using Core.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IDal.Database
{
    public interface IDbReactionRole
    {
        Task Add(ReactionRole value);
        Task Update(ReactionRole value);
        Task Remove(ReactionRole value);
        Task<ReactionRole> Get(ulong id);
        Task<List<ReactionRole>> List(ulong serverId);
    }
}