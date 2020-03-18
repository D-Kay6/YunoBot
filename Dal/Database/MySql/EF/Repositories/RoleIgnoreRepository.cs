namespace Dal.Database.MySql.EF.Repositories
{
    using System.Threading.Tasks;
    using Core.Entity;
    using IDal.Database;
    using Microsoft.EntityFrameworkCore;

    public class RoleIgnoreRepository : BaseRepository, IDbRoleIgnore
    {
        public async Task Add(RoleIgnore value)
        {
            Context.IgnoredUsers.Add(value);
            await Context.SaveChangesAsync();
        }

        public async Task Update(RoleIgnore value)
        {
            Context.IgnoredUsers.Update(value);
            await Context.SaveChangesAsync();
        }

        public async Task Remove(RoleIgnore value)
        {
            Context.IgnoredUsers.Remove(value);
            await Context.SaveChangesAsync();
        }

        public async Task<RoleIgnore> Get(ulong serverId, ulong userId)
        {
            return await Context.IgnoredUsers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ServerId == serverId && x.UserId == userId);
        }
    }
}