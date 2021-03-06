﻿using Core.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IDal.Database
{
    public interface IDbReactionRoleData
    {
        Task Add(ReactionRoleData value);
        Task Update(ReactionRoleData value);
        Task Remove(ReactionRoleData value);
        Task<ReactionRoleData> Get(ulong reactionRoleId, ulong roleId);
    }
}