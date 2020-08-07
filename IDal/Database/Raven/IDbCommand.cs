using Entity.RavenDB;
using System.Threading.Tasks;

namespace IDal.Database.Raven
{
    public interface IDbCommand
    {
        Task Add(CommandSetting value);
        Task Update(CommandSetting value);
        Task Remove(string id);
        Task<CommandSetting> Get(ulong serverId);
    }
}