using Entity;
using IDal.Database;
using System.Threading.Tasks;

namespace Dal.EF.MultiThreaded
{
    public class RoleRepository : MultiThreadedRepository, IDbRole
    {
        private readonly SingleThreaded.RoleRepository _repository;

        public RoleRepository()
        {
            _repository = new SingleThreaded.RoleRepository();
        }

        public async Task<bool> IsAutoEnabled(ulong serverId)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.IsAutoEnabled(serverId));
        }

        public async Task<bool> SetAutoEnabled(ulong serverId, bool enabled)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.SetAutoEnabled(serverId, enabled));
        }

        public async Task<string> GetAutoPrefix(ulong serverId)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.GetAutoPrefix(serverId));
        }

        public async Task<bool> SetAutoPrefix(ulong serverId, string prefix)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.SetAutoPrefix(serverId, prefix));
        }


        public async Task<bool> IsPermaEnabled(ulong serverId)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.IsPermaEnabled(serverId));
        }

        public async Task<bool> SetPermaEnabled(ulong serverId, bool enabled)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.SetPermaEnabled(serverId, enabled));
        }

        public async Task<string> GetPermaPrefix(ulong serverId)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.GetPermaPrefix(serverId));
        }

        public async Task<bool> SetPermaPrefix(ulong serverId, string prefix)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.SetPermaPrefix(serverId, prefix));
        }


        public async Task<bool> IsGeneratedChannel(ulong serverId, ulong channelId)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.IsGeneratedChannel(serverId, channelId));
        }

        public async Task<bool> AddGeneratedChannel(ulong serverId, ulong channelId)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.AddGeneratedChannel(serverId, channelId));
        }

        public async Task<bool> RemoveGeneratedChannel(ulong serverId, ulong channelId)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.RemoveGeneratedChannel(serverId, channelId));
        }


        public async Task<bool> IsIgnoringRoles(ulong serverId, ulong userId)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.IsIgnoringRoles(serverId, userId));
        }

        public async Task<bool> AddIgnoringRoles(ulong serverId, ulong userId)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.AddIgnoringRoles(serverId, userId));
        }

        public async Task<bool> RemoveIgnoringRoles(ulong serverId, ulong userId)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.RemoveIgnoringRoles(serverId, userId));
        }


        public async Task<AutoRole> GetAutoChannel(ulong serverId)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.GetAutoChannel(serverId));
        }

        public async Task<PermaRole> GetPermaChannel(ulong serverId)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.GetPermaChannel(serverId));
        }
    }
}