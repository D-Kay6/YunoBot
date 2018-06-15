using System.Threading.Tasks;
using Discord.WebSocket;
using ILogic.Interfaces;

namespace Logic.Music
{
    internal class MusicHandler : IHandler
    {
        private DiscordSocketClient _client;

        public async Task Initialize(DiscordSocketClient client, IConfig config)
        {
            this._client = client;
        }
    }
}
