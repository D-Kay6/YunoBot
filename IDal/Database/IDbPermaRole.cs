using Core.Entity;
using System.Threading.Tasks;

namespace IDal.Database
{
    public interface IDbPermaRole
    {
        Task Add(PermaRole value);
        Task Update(PermaRole value);
        Task Remove(PermaRole value);
        Task<PermaRole> Get(ulong serverId);
    }
}