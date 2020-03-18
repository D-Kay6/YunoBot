namespace IDal.Database
{
    using System.Threading.Tasks;
    using Core.Entity;

    public interface IDbPermaChannel
    {
        Task Add(PermaChannel value);
        Task Update(PermaChannel value);
        Task Remove(PermaChannel value);
        Task<PermaChannel> Get(ulong serverId);
    }
}