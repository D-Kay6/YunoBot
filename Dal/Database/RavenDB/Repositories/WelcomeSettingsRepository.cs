namespace Dal.Database.RavenDB.Repositories
{
    using System;
    using System.Threading.Tasks;
    using Entity.RavenDB;
    using IDal.Database.Raven;

    public class WelcomeSettingsRepository : BaseRepository, IDbWelcome
    {
        public Task Add(WelcomeMessage value)
        {
            throw new NotImplementedException();
        }

        public Task Update(WelcomeMessage value)
        {
            throw new NotImplementedException();
        }

        public Task Remove(string id)
        {
            throw new NotImplementedException();
        }

        public Task<WelcomeMessage> Get(ulong serverId)
        {
            throw new NotImplementedException();
        }
    }
}