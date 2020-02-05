using System.Collections.Generic;
using System.Threading.Tasks;
using Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;

namespace Dal.Database.MySql.EF.SingleThreaded
{
    public class UserRepository : BaseRepository, IDbUser
    {
        public async Task AddUser(ulong id, string name)
        {
            Context.Users.Add(new User
            {
                Id = id,
                Name = name
            });
            await Context.SaveChangesAsync();
        }

        public async Task AddUser(User user)
        {
            try
            {
                Context.Users.Add(user);
                await Context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {

            }
        }

        public async Task UpdateUser(ulong id, string name)
        {
            var user = await Context.Users.FindAsync(id);
            if (user == null)
            {
                await AddUser(id, name);
                return;
            }

            user.Name = name;
            await Context.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            Context.Users.Update(user);
            await Context.SaveChangesAsync();
        }

        public async Task<User> GetUser(ulong id)
        {
            return await Context.Users.FindAsync(id);
        }

        public async Task<List<User>> GetUsers()
        {
            return await Context.Users.ToListAsync();
        }
    }
}