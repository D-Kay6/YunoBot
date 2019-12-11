using Entity;

namespace IDal.Interfaces.Database
{
    public interface IChannel
    {
        bool IsAutoEnabled(ulong serverId);
        bool SetAutoEnabled(ulong serverId, bool enabled);
        string GetAutoPrefix(ulong serverId);
        bool SetAutoPrefix(ulong serverId, string prefix);
        string GetAutoName(ulong serverId);
        bool SetAutoName(ulong serverId, string name);

        bool IsPermaEnabled(ulong serverId);
        bool SetPermaEnabled(ulong serverId, bool enabled);
        string GetPermaPrefix(ulong serverId);
        bool SetPermaPrefix(ulong serverId, string prefix);
        string GetPermaName(ulong serverId);
        bool SetPermaName(ulong serverId, string name);

        bool IsGeneratedChannel(ulong serverId, ulong channelId);
        bool AddGeneratedChannel(ulong serverId, ulong channelId);
        bool RemoveGeneratedChannel(ulong serverId, ulong channelId);

        AutoChannel GetAutoChannel(ulong serverId);
        PermaChannel GetPermaChannel(ulong serverId);
    }
}
