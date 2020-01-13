using System.Threading.Tasks;
using Entity;

namespace IDal.Database
{
    public interface IDbRole
    {
        Task<bool> IsAutoEnabled(ulong serverId);
        Task<bool> SetAutoEnabled(ulong serverId, bool enabled);
        Task<string> GetAutoPrefix(ulong serverId);
        Task<bool> SetAutoPrefix(ulong serverId, string prefix);

        Task<bool> IsPermaEnabled(ulong serverId);
        Task<bool> SetPermaEnabled(ulong serverId, bool enabled);
        Task<string> GetPermaPrefix(ulong serverId);
        Task<bool> SetPermaPrefix(ulong serverId, string prefix);

        Task<bool> IsGeneratedChannel(ulong serverId, ulong channelId);
        Task<bool> AddGeneratedChannel(ulong serverId, ulong channelId);
        Task<bool> RemoveGeneratedChannel(ulong serverId, ulong channelId);

        Task<bool> IsIgnoringRoles(ulong serverId, ulong userId);
        Task<bool> AddIgnoringRoles(ulong serverId, ulong userId);
        Task<bool> RemoveIgnoringRoles(ulong serverId, ulong userId);

        Task<AutoRole> GetAutoChannel(ulong serverId);
        Task<PermaRole> GetPermaChannel(ulong serverId);
    }
}
