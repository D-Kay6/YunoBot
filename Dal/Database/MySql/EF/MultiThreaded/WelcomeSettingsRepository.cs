using System.Threading.Tasks;
using Entity;
using IDal.Database;

namespace Dal.Database.MySql.EF.MultiThreaded
{
    public class WelcomeSettingsRepository : MultiThreadedRepository, IDbWelcome
    {
        private readonly SingleThreaded.WelcomeSettingsRepository _repository;

        public WelcomeSettingsRepository()
        {
            _repository = new SingleThreaded.WelcomeSettingsRepository();
        }

        public async Task<bool> Enable(ulong serverId, ulong channelId)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.Enable(serverId, channelId));
        }

        public async Task<bool> Disable(ulong serverId)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.Disable(serverId));
        }

        public async Task<bool> UseImage(ulong serverId, bool value)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.UseImage(serverId, value));
        }

        public async Task<bool> SetWelcomeMessage(ulong serverId, string message)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.SetWelcomeMessage(serverId, message));
        }

        public async Task<WelcomeMessage> GetWelcomeSettings(ulong serverId)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.GetWelcomeSettings(serverId));
        }
    }
}