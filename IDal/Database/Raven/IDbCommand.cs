namespace IDal.Database.Raven
{
    using System.Threading.Tasks;
    using Entity.RavenDB;

    public interface IDbCommand
    {
        Task Add(CommandSetting value);
        Task Update(CommandSetting value);
        Task Remove(string id);
        Task<CommandSetting> Get(ulong serverId);
    }
}