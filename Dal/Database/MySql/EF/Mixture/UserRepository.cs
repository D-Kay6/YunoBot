using Core.Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.Mixture
{
    public class UserRepository : IDbUser
    {
        public async Task AddUser(ulong id, string name)
        {
            await using var context = new DataContext();
            context.Users.Add(new User
            {
                Id = id,
                Name = name
            });
            await context.SaveChangesAsync();
        }

        public async Task AddUser(User user)
        {
            await using var context = new DataContext();
            try
            {
                context.Users.Add(user);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {

            }
        }

        public async Task UpdateUser(ulong id, string name)
        {
            await using var context = new DataContext();
            var user = await context.Users.FindAsync(id);
            if (user == null)
            {
                await AddUser(id, name);
                return;
            }

            user.Name = name;
            await context.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            await using var context = new DataContext();
            context.Users.Update(user);
            await context.SaveChangesAsync();
        }

        public async Task<User> GetUser(ulong id)
        {
            await using var context = new DataContext();
            return await context.Users.FindAsync(id);
        }

        public async Task<List<User>> GetUsers()
        {
            await using var context = new DataContext();
            return await context.Users.ToListAsync();
        }
    }
}