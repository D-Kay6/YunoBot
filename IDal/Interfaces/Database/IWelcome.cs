using Entity;

namespace IDal.Interfaces.Database
{
    public interface IWelcome
    {
        bool Enable(ulong serverId, ulong channelId);
        bool Disable(ulong serverId);
        bool UseImage(ulong serverId, bool value);
        bool SetWelcomeMessage(ulong serverId, string message);
        WelcomeMessage GetWelcomeSettings(ulong serverId);
    }
}