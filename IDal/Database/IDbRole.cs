using Core.Entity;
using System.Threading.Tasks;

namespace IDal.Database
{
    public interface IDbRole
    {
        Task Add(Role value);
        Task Update(Role value);
        Task Remove(Role value);
        Task<Role> Get(ulong id);
    }
}