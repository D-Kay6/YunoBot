using Core.Entity;
using System.Threading.Tasks;

namespace IDal.Database
{
    public interface IDbAutoChannel
    {
        Task Add(AutoChannel value);
        Task Update(AutoChannel value);
        Task Remove(AutoChannel value);
        Task<AutoChannel> Get(ulong serverId);
    }
}