using Core.Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Repositories
{
    public class UserRepository : BaseRepository, IDbUser
    {
        public async Task Add(User value)
        {
            Context.Users.Add(value);
            await Context.SaveChangesAsync();
        }

        public async Task Update(User value)
        {
            try
            {
                var entry = Context.Attach(value);
                entry.State = EntityState.Modified;
            }
            catch
            {
                //ignore
            }

            Context.Users.Update(value);
            await Context.SaveChangesAsync();
        }

        public async Task Remove(User value)
        {
            Context.Users.Remove(value);
            await Context.SaveChangesAsync();
        }

        public async Task<User> Get(ulong id)
        {
            return await Context.Users.FindAsync(id);
        }
    }
}