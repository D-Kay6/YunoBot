using Discord.WebSocket;
using System.Threading.Tasks;

namespace ILogic.Interfaces
{
    public interface IHandler
    {
        Task Initialize(DiscordSocketClient client, IConfig config);
    }
}
