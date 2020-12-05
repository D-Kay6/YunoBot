using Core.Entity;
using System.Threading.Tasks;

namespace IDal.Database
{
    public interface IDbUser
    {
        Task Add(User value);
        Task Update(User value);
        Task Remove(User value);
        Task<User> Get(ulong id);
    }
}