namespace IDal.Database
{
    using System.Threading.Tasks;
    using Core.Entity;

    public interface IDbServer
    {
        Task Add(Server value);
        Task Update(Server value);
        Task Remove(Server value);
        Task<Server> Get(ulong serverId);
    }
}