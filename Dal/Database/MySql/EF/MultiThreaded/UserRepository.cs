using Core.Entity;
using IDal.Database;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.MultiThreaded
{
    public class UserRepository : MultiThreadedRepository, IDbUser
    {
        private readonly SingleThreaded.UserRepository _repository;

        public UserRepository()
        {
            _repository = new SingleThreaded.UserRepository();
        }

        public async Task AddUser(ulong id, string name)
        {
            await Semaphore.WaitAsync();
            await Execute(_repository.AddUser(id, name));
        }

        public async Task AddUser(User user)
        {
            await Semaphore.WaitAsync();
            await Execute(_repository.AddUser(user));
        }

        public async Task UpdateUser(ulong id, string name)
        {
            await Semaphore.WaitAsync();
            await Execute(_repository.UpdateUser(id, name));
        }

        public async Task UpdateUser(User user)
        {
            await Semaphore.WaitAsync();
            await Execute(_repository.UpdateUser(user));
        }

        public async Task<User> GetUser(ulong id)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.GetUser(id));
        }

        public async Task<List<User>> GetUsers()
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.GetUsers());
        }
    }
}