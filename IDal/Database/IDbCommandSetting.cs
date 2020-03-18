namespace IDal.Database
{
    using System.Threading.Tasks;
    using Core.Entity;

    public interface IDbCommandSetting
    {
        Task Add(CommandSetting value);
        Task Update(CommandSetting value);
        Task Remove(CommandSetting value);
        Task<CommandSetting> Get(ulong serverId);
    }
}