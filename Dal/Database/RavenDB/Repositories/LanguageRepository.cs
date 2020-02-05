using Entity.RavenDB;
using IDal.Database.Raven;
using System.Threading.Tasks;

namespace Dal.Database.RavenDB.Repositories
{
    public class LanguageRepository : BaseRepository, IDbLanguage
    {
        public Task Add(LanguageSetting value)
        {
            throw new System.NotImplementedException();
        }

        public Task Update(LanguageSetting value)
        {
            throw new System.NotImplementedException();
        }

        public Task Delete(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<LanguageSetting> Get(ulong serverId)
        {
            throw new System.NotImplementedException();
        }
    }
}