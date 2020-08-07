using Core.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace IDal.Database
{
    public interface IDbGeneratedChannel
    {
        Task Add(GeneratedChannel value);
        Task Update(GeneratedChannel value);
        Task Remove(GeneratedChannel value);
        Task<GeneratedChannel> Get(ulong serverId, ulong channelId);
        IQueryable<GeneratedChannel> Query(ulong serverId);
    }
}