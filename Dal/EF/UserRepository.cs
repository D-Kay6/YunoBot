using Entity;
using IDal.Database;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal.EF
{
    public class UserRepository : IDbUser
    {
        public async Task AddUser(ulong id, string name)
        {
            var context = new DataContext();
            try
            {
                context.Users.Add(new User
                {
                    Id = id,
                    Name = name
                });
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {

            }
        }

        public async Task AddUser(User user)
        {
            var context = new DataContext();
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
            var context = new DataContext();
            try
            {
                var user = await context.Users.FindAsync(id);
                if (user == null)
                {
                    await AddUser(id, name);
                    return;
                }

                user.Name = name;
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {

            }
        }

        public async Task UpdateUser(User user)
        {
            var context = new DataContext();
            try
            {
                context.Users.Update(user);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {

            }
        }

        public async Task<User> GetUser(ulong id)
        {
            var context = new DataContext();
            try
            {
                return await context.Users.FindAsync(id);
            }
            catch (DbUpdateException)
            {
                return null;
            }
        }

        public async Task<List<User>> GetUsers()
        {
            var context = new DataContext();
            try
            {
                return await context.Users.ToListAsync();
            }
            catch (DbUpdateException)
            {
                return null;
            }
        }
    }
}
