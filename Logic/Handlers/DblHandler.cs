using Discord.WebSocket;
using DiscordBotsList.Api;
using System;
using System.Threading.Tasks;
using IDal.Structs.Configuration;

namespace Logic.Handlers
{
    public class DblHandler
    {
        private DiscordSocketClient _client;
        public AuthDiscordBotListApi DblApi { get; private set; }

        public async Task Initialize(DiscordSocketClient client, ConfigData config)
        {
            _client = client;
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