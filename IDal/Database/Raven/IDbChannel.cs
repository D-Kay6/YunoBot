using Entity.RavenDB;
using System.Threading.Tasks;

namespace IDal.Database.Raven
{
    public interface IDbChannel
    {
        Task Add(ChannelAutomation value);
        Task Update(ChannelAutomation value);
        Task Remove(string id);
        Task<ChannelAutomation> Get(ulong serverId);
    }
}