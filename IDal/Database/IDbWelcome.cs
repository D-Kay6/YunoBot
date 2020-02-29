using Core.Entity;
using System.Threading.Tasks;

namespace IDal.Database
{
    public interface IDbWelcome
    {
        Task<bool> Enable(ulong serverId, ulong channelId);
        Task<bool> Disable(ulong serverId);
        Task<bool> UseImage(ulong serverId, bool value);
        Task<bool> SetWelcomeMessage(ulong serverId, string message);
        Task<WelcomeMessage> GetWelcomeSettings(ulong serverId);
    }
}