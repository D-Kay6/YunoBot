using IDal.Structs.Database;

namespace IDal.Interfaces.Database
{
    public interface IAutoChannel
    {
        bool IsAutoChannel(ulong serverId, string name);
        bool IsPermaChannel(ulong serverId, string name);

        bool IsGeneratedChannel(ulong serverId, ulong channelId);
        bool AddGeneratedChannel(ulong serverId, ulong channelId);
        bool RemoveGeneratedChannel(ulong serverId, ulong channelId);
        bool ClearGeneratedChannels(ulong serverId);

        bool SetAutoPrefix(ulong serverId, string prefix);
        bool SetAutoName(ulong serverId, string name);
        bool SetPermaPrefix(ulong serverId, string prefix);
        bool SetPermaName(ulong serverId, string name);

        ChannelData GetData(ulong serverId);
    }
}