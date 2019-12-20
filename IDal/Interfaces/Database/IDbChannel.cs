using System.Threading.Tasks;
using Entity;

namespace IDal.Interfaces.Database
{
    public interface IDbChannel
    {
        Task<bool> IsAutoEnabled(ulong serverId);
        Task<bool> SetAutoEnabled(ulong serverId, bool enabled);
        Task<string> GetAutoPrefix(ulong serverId);
        Task<bool> SetAutoPrefix(ulong serverId, string prefix);
        Task<string> GetAutoName(ulong serverId);
        Task<bool> SetAutoName(ulong serverId, string name);

        Task<bool> IsPermaEnabled(ulong serverId);
        Task<bool> SetPermaEnabled(ulong serverId, bool enabled);
        Task<string> GetPermaPrefix(ulong serverId);
        Task<bool> SetPermaPrefix(ulong serverId, string prefix);
        Task<string> GetPermaName(ulong serverId);
        Task<bool> SetPermaName(ulong serverId, string name);

        Task<bool> IsGeneratedChannel(ulong serverId, ulong channelId);
        Task<bool> AddGeneratedChannel(ulong serverId, ulong channelId);
        Task<bool> RemoveGeneratedChannel(ulong serverId, ulong channelId);

        Task<AutoChannel> GetAutoChannel(ulong serverId);
        Task<PermaChannel> GetPermaChannel(ulong serverId);
    }
}
