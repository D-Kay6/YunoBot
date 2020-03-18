namespace IDal.Database
{
    using System.Threading.Tasks;
    using Core.Entity;

    public interface IDbAutoChannel
    {
        Task Add(AutoChannel value);
        Task Update(AutoChannel value);
        Task Remove(AutoChannel value);
        Task<AutoChannel> Get(ulong serverId);
    }
}