using System;
using System.Threading.Tasks;
using Discord.WebSocket;
using DiscordBotsList.Api;
using Yuno.Data.Core.Structs;

namespace Yuno.Main.DiscordBotList
{
    public class DblHandler
    {
        public AuthDiscordBotListApi DblApi { get; private set; }

        private DiscordSocketClient _client;

        public async Task Initialize(DiscordSocketClient client, ConfigData config)
        {
            this._client = client;
            DblApi = new AuthDiscordBotListApi(config.ClientId, config.DiscordBotsToken);
            client.Ready += OnClientReady;
            client.JoinedGuild += OnGuildJoined;
            client.LeftGuild += OnGuildLeft;
        }

        private async Task UpdateGuilds()
        {
            Console.WriteLine($"Updating guilds to {_client.Guilds.Count}");
            var me = await DblApi.GetMeAsync();
            await me.UpdateStatsAsync(_client.Guilds.Count);
            await DblApi.UpdateStats(_client.Guilds.Count);
        }

        private async Task OnClientReady()
        {
            await UpdateGuilds();
        }

        private async Task OnGuildJoined(SocketGuild arg)
        {
            await UpdateGuilds();
        }

        private async Task OnGuildLeft(SocketGuild arg)
        {
            await UpdateGuilds();
        }
    }
}
