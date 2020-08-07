using Entity.RavenDB;
using IDal.Database.Raven;
using System;
using System.Threading.Tasks;

namespace Dal.Database.RavenDB.Repositories
{
    public class CommandRepository : BaseRepository, IDbCommand
    {
        public Task Add(CommandSetting value)
        {
            throw new NotImplementedException();
        }

        public Task Update(CommandSetting value)
        {
            throw new NotImplementedException();
        }

        public Task Remove(string id)
        {
            throw new NotImplementedException();
        }

        public Task<CommandSetting> Get(ulong serverId)
        {
            throw new NotImplementedException();
        }
    }
}