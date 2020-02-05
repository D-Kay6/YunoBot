using System.Collections.Generic;
using System.Threading.Tasks;
using Entity;
using IDal.Database;

namespace Dal.Database.MySql.EF.MultiThreaded
{
    public class CommandRepository : MultiThreadedRepository, IDbCommand
    {
        private readonly SingleThreaded.CommandRepository _repository;

        public CommandRepository()
        {
            _repository = new SingleThreaded.CommandRepository();
        }

        public async Task<string> GetPrefix(ulong serverId)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.GetPrefix(serverId));
        }

        public async Task<bool> SetPrefix(ulong serverId, string prefix)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.SetPrefix(serverId, prefix));
        }

        public async Task<bool> AddCustomCommand(ulong serverId, string command, string response)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.AddCustomCommand(serverId, command, response));
        }

        public async Task<bool> AddCustomCommand(CustomCommand customCommand)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.AddCustomCommand(customCommand));
        }

        public async Task<bool> RemoveCustomCommand(ulong serverId, string command)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.RemoveCustomCommand(serverId, command));
        }

        public async Task<bool> RemoveCustomCommand(CustomCommand customCommand)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.RemoveCustomCommand(customCommand));
        }

        public async Task<CustomCommand> GetCustomCommand(ulong serverId, string command)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.GetCustomCommand(serverId, command));
        }

        public async Task<List<CustomCommand>> GetCustomCommands(ulong serverId)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.GetCustomCommands(serverId));
        }
    }
}