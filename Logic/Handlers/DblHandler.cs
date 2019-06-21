using Discord.WebSocket;
using DiscordBotsList.Api;
using IDal.Structs.Configuration;
using System;
using System.Threading.Tasks;

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
            try
            {
                var me = await DblApi.GetMeAsync();
                await me.UpdateStatsAsync(_client.Guilds.Count);
                await DblApi.UpdateStats(_client.Guilds.Count);
                Console.WriteLine($"Updating guilds to {_client.Guilds.Count}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not update guild count. {e.Message}");
            }
        }

        private async Task OnClientReady()
        {
            await UpdateGuilds();
        }

        private async Task OnGuildJoined(SocketGuild guild)
        {
            await UpdateGuilds();
            LogsHandler.Instance.Log("Connections", guild, "Joined.");
        }

        private async Task OnGuildLeft(SocketGuild guild)
        {
            await UpdateGuilds();
            LogsHandler.Instance.Log("Connections", guild, "Left.");
        }
    }
}