using Entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDal.Database;

namespace Dal.EF
{
    public class UserRepository : IDbUser
    {
        private DataContext _context;

        public UserRepository()
        {
            _context = new DataContext();
        }

        public async Task AddUser(ulong id, string name)
        {
            _context.Users.Add(new User
            {
                Id = id,
                Name = name
            });
            await _context.SaveChangesAsync();
        }

        public async Task AddUser(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {

            }
        }

        public async Task UpdateUser(ulong id, string name)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                await AddUser(id, name);
                return;
            }

            user.Name = name;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<User> GetUser(ulong id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<List<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
