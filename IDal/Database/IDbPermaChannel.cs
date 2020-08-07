using Core.Entity;
using System.Threading.Tasks;

namespace IDal.Database
{
    public interface IDbPermaChannel
    {
        Task Add(PermaChannel value);
        Task Update(PermaChannel value);
        Task Remove(PermaChannel value);
        Task<PermaChannel> Get(ulong serverId);
    }
}