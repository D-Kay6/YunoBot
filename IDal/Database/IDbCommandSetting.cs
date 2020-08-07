using Core.Entity;
using System.Threading.Tasks;

namespace IDal.Database
{
    public interface IDbCommandSetting
    {
        Task Add(CommandSetting value);
        Task Update(CommandSetting value);
        Task Remove(CommandSetting value);
        Task<CommandSetting> Get(ulong serverId);
    }
}