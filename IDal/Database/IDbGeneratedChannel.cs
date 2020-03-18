namespace IDal.Database
{
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Entity;

    public interface IDbGeneratedChannel
    {
        Task Add(GeneratedChannel value);
        Task Update(GeneratedChannel value);
        Task Remove(GeneratedChannel value);
        Task<GeneratedChannel> Get(ulong serverId, ulong channelId);
        IQueryable<GeneratedChannel> Query(ulong serverId);
    }
}