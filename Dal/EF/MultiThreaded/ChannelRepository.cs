using Entity;
using IDal.Database;
using System.Threading.Tasks;

namespace Dal.EF.MultiThreaded
{
    public class ChannelRepository : MultiThreadedRepository, IDbChannel
    {
        private readonly SingleThreaded.ChannelRepository _repository;

        public ChannelRepository()
        {
            _repository = new SingleThreaded.ChannelRepository();
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

        public async Task<string> GetAutoName(ulong serverId)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.GetAutoName(serverId));
        }

        public async Task<bool> SetAutoName(ulong serverId, string name)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.SetAutoName(serverId, name));
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

        public async Task<string> GetPermaName(ulong serverId)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.GetPermaName(serverId));
        }

        public async Task<bool> SetPermaName(ulong serverId, string name)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.SetPermaName(serverId, name));
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


        public async Task<AutoChannel> GetAutoChannel(ulong serverId)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.GetAutoChannel(serverId));
        }

        public async Task<PermaChannel> GetPermaChannel(ulong serverId)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.GetPermaChannel(serverId));
        }
    }
}