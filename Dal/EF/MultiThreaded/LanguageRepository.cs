using Entity;
using IDal.Database;
using System.Threading.Tasks;

namespace Dal.EF.MultiThreaded
{
    public class LanguageRepository : MultiThreadedRepository, IDbLanguage
    {
        private readonly SingleThreaded.LanguageRepository _repository;

        public LanguageRepository()
        {
            _repository = new SingleThreaded.LanguageRepository();
        }

        public async Task<bool> SetLanguage(ulong serverId, Language language)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.SetLanguage(serverId, language));
        }

        public async Task<Language> GetLanguage(ulong serverId)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.GetLanguage(serverId));
        }
    }
}