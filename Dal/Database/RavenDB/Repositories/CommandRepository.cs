using Entity.RavenDB;
using IDal.Database.Raven;
using System.Threading.Tasks;

namespace Dal.Database.RavenDB.Repositories
{
    public class CommandRepository : BaseRepository, IDbCommand
    {
        public Task Add(CommandSetting value)
        {
            throw new System.NotImplementedException();
        }

        public Task Update(CommandSetting value)
        {
            throw new System.NotImplementedException();
        }

        public Task Remove(string id)
        {
            throw new System.NotImplementedException();
        }

        public Task<CommandSetting> Get(ulong serverId)
        {
            throw new System.NotImplementedException();
        }
    }
}