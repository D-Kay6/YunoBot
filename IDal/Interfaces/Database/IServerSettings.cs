using IDal.Structs.Localization;

namespace IDal.Interfaces.Database
{
    public interface IServerSettings
    {
        bool RegisterServer(ulong serverId, string name);
        bool UpdateServer(ulong serverId, string name);
        bool DeleteServer(ulong serverId);

        bool SetLanguage(ulong serverId, Language language);
        Language GetLanguage(ulong serverId);

        bool SetCommandPrefix(ulong serverId, string prefix);
        string GetCommandPrefix(ulong serverId);
    }
}