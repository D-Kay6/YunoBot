using Core.Entity;
using System.Threading.Tasks;

namespace IDal.Database
{
    public interface IDbAutoRole
    {
        Task Add(AutoRole value);
        Task Update(AutoRole value);
        Task Remove(AutoRole value);
        Task<AutoRole> Get(ulong serverId);
    }
}