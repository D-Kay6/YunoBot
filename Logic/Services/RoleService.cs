namespace Logic.Services
{
    using System.Threading.Tasks;
    using Core.Entity;

    public class RoleService
    {
        private Task<bool> IsAutoEnabled(ulong serverId);
        private Task<bool> SetAutoEnabled(ulong serverId, bool enabled);
        private Task<string> GetAutoPrefix(ulong serverId);
        private Task<bool> SetAutoPrefix(ulong serverId, string prefix);

        private Task<bool> IsPermaEnabled(ulong serverId);
        private Task<bool> SetPermaEnabled(ulong serverId, bool enabled);
        private Task<string> GetPermaPrefix(ulong serverId);
        private Task<bool> SetPermaPrefix(ulong serverId, string prefix);

        private Task<bool> IsGeneratedChannel(ulong serverId, ulong channelId);
        private Task<bool> AddGeneratedChannel(ulong serverId, ulong channelId);
        private Task<bool> RemoveGeneratedChannel(ulong serverId, ulong channelId);

        private Task<bool> IsIgnoringRoles(ulong serverId, ulong userId);
        private Task<bool> AddIgnoringRoles(ulong serverId, ulong userId);
        private Task<bool> RemoveIgnoringRoles(ulong serverId, ulong userId);

        private Task<AutoRole> GetAutoChannel(ulong serverId);
        private Task<PermaRole> GetPermaChannel(ulong serverId);
    }
}