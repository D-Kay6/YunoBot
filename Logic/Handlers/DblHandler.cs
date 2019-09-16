using Discord.WebSocket;
using DiscordBotsList.Api;
using IDal.Structs.Configuration;
using System;
using System.Threading.Tasks;
using Logic.Services;

namespace Logic.Handlers
{
    public class DblHandler : BaseHandler
    {
        public AuthDiscordBotListApi DblApi { get; private set; }

        public DblHandler(DiscordSocketClient client, IServiceProvider serviceProvider, ConfigData config) : base(client, serviceProvider)
        {
            DblApi = new AuthDiscordBotListApi(config.ClientId, config.DiscordBotsToken);
        }

        public override async Task Initialize()
        {
            Client.JoinedGuild += OnGuildJoined;
            Client.LeftGuild += OnGuildLeft;

            await UpdateGuilds();
        }

        private async Task UpdateGuilds()
        {
            try
            {
                var me = await DblApi.GetMeAsync();
                await me.UpdateStatsAsync(Client.Guilds.Count);
                await DblApi.UpdateStats(Client.Guilds.Count);
                Console.WriteLine($"Updating guilds to {Client.Guilds.Count}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not update guild count. {e.Message}");
            }
        }

        private async Task OnGuildJoined(SocketGuild guild)
        {
            await UpdateGuilds();
            LogService.Instance.Log("Connections", guild, "Joined.");
        }

        private async Task OnGuildLeft(SocketGuild guild)
        {
            await UpdateGuilds();
            LogService.Instance.Log("Connections", guild, "Left.");
        }
    }
}