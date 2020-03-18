namespace Dal.Database.MySql.EF.Repositories
{
    using System.Threading.Tasks;
    using Core.Entity;
    using IDal.Database;
    using Microsoft.EntityFrameworkCore;

    public class AutoRoleRepository : BaseRepository, IDbAutoRole
    {
        public async Task Add(AutoRole value)
        {
            Context.AutoRoles.Add(value);
            await Context.SaveChangesAsync();
        }

        public async Task Update(AutoRole value)
        {
            Context.AutoRoles.Update(value);
            await Context.SaveChangesAsync();
        }

        public async Task Remove(AutoRole value)
        {
            Context.AutoRoles.Remove(value);
            await Context.SaveChangesAsync();
        }

        public async Task<AutoRole> Get(ulong serverId)
        {
            return await Context.AutoRoles
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.ServerId == serverId);
        }
    }
}