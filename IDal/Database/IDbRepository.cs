using System.Linq;
using System.Threading.Tasks;

namespace IDal.Database
{
    public interface IDbRepository<T>
    {
        Task Add(T value);
        Task Update(T value);
        Task Remove(T value);
        Task<T> Get(ulong serverId);
        IQueryable<T> Query(ulong serverId);
    }
}