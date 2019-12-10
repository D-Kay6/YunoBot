using Entity;

namespace IDal.Interfaces.Database
{
    public interface IRole
    {
        bool IsAutoEnabled(ulong serverId);
        bool SetAutoEnabled(ulong serverId, bool enabled);
        string GetAutoPrefix(ulong serverId);
        bool SetAutoPrefix(ulong serverId, string prefix);

        bool IsPermaEnabled(ulong serverId);
        bool SetPermaEnabled(ulong serverId, bool enabled);
        string GetPermaPrefix(ulong serverId);
        bool SetPermaPrefix(ulong serverId, string prefix);

        bool IsGeneratedChannel(ulong serverId, ulong channelId);
        bool AddGeneratedChannel(ulong serverId, ulong channelId);
        bool RemoveGeneratedChannel(ulong serverId, ulong channelId);

        bool IsIgnoringRoles(ulong serverId, ulong userId);
        bool AddIgnoringRoles(ulong serverId, ulong userId);
        bool RemoveIgnoringRoles(ulong serverId, ulong userId);

        AutoRole GetAutoChannel(ulong serverId);
        PermaRole GetPermaChannel(ulong serverId);
    }
}
