namespace Dal.Database.RavenDB.Repositories
{
    using System;
    using System.Threading.Tasks;
    using Entity.RavenDB;
    using IDal.Database.Raven;

    public class LanguageRepository : BaseRepository, IDbLanguage
    {
        public Task Add(LanguageSetting value)
        {
            throw new NotImplementedException();
        }

        public Task Update(LanguageSetting value)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        public Task<LanguageSetting> Get(ulong serverId)
        {
            throw new NotImplementedException();
        }
    }
}