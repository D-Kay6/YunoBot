using System.Threading.Tasks;
using Entity;
using IDal.Database;

namespace Dal.Database.MySql.EF.MultiThreaded
{
    public class ServerRepository : MultiThreadedRepository, IDbServer
    {
        private readonly SingleThreaded.ServerRepository _repository;

        public ServerRepository()
        {
            _repository = new SingleThreaded.ServerRepository();
        }

        public async Task AddServer(ulong id, string name)
        {
            await Semaphore.WaitAsync();
            await Execute(_repository.AddServer(id, name));
        }

        public async Task AddServer(Server server)
        {
            await Semaphore.WaitAsync();
            await Execute(_repository.AddServer(server));
        }

        public async Task UpdateServer(ulong id, string name)
        {
            await Semaphore.WaitAsync();
            await Execute(_repository.UpdateServer(id, name));
        }

        public async Task UpdateServer(Server server)
        {
            await Semaphore.WaitAsync();
            await Execute(_repository.UpdateServer(server));
        }

        public async Task DeleteServer(ulong id)
        {
            await Semaphore.WaitAsync();
            await Execute(_repository.DeleteServer(id));
        }

        public async Task<Server> GetServer(ulong id)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.GetServer(id));
        }

        public async Task Save()
        {
            await Semaphore.WaitAsync();
            await Execute(_repository.Save());
        }
    }
}