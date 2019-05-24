using IDal.Structs.Database;

namespace IDal.Interfaces.Database
{
    public interface IWelcomeMessage
    {
        bool Enable(ulong serverId, ulong channelId);
        bool Disable(ulong serverId);
        bool UseImage(ulong serverId, bool value);
        bool SetWelcomeMessage(ulong serverId, string message);
        WelcomeData GetWelcomeMessage(ulong serverId);
    }
}