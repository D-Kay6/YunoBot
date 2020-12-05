using Core.Entity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IDal.Database
{
    public interface IDbCommandCustom
    {
        Task Add(CustomCommand value);
        Task Update(CustomCommand value);
        Task Remove(CustomCommand value);
        Task<CustomCommand> Get(ulong serverId, string command);
        Task<List<CustomCommand>> List(ulong serverId);
    }
}