namespace Dal.Database.MySql.EF.Repositories
{
    using System.Threading.Tasks;
    using Core.Entity;
    using IDal.Database;
    using Microsoft.EntityFrameworkCore;

    public class PermaRoleRepository : BaseRepository, IDbPermaRole
    {
        public async Task Add(PermaRole value)
        {
            Context.PermaRoles.Add(value);
            await Context.SaveChangesAsync();
        }

        public async Task Update(PermaRole value)
        {
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
            return await Context.PermaRoles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ServerId == serverId);
        }
    }
}