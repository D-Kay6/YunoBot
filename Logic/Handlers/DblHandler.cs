using Discord.WebSocket;
using DiscordBotsList.Api;
using IDal;
using Logic.Services;
using System;
using System.Threading.Tasks;

namespace Logic.Handlers
{
    public class DblHandler : BaseHandler
    {
        private IConfig _config;
        private LogsService _logs;

        public AuthDiscordBotListApi DblApi { get; private set; }

        public DblHandler(DiscordSocketClient client, IConfig config, LogsService logs) : base(client)
        {
            _config = config;
            _logs = logs;
        }

        public override async Task Initialize()
        {
            var settings = await _config.Read();
            DblApi = new AuthDiscordBotListApi(settings.ClientId, settings.DiscordBotsToken);

            Client.Ready += OnReady;
        }

        private async Task OnReady()
        {
            Client.JoinedGuild += OnGuildJoined;
            Client.LeftGuild += OnGuildLeft;

            await UpdateGuilds().ConfigureAwait(false);
        }

        private async Task OnGuildJoined(SocketGuild guild)
        {
            await UpdateGuilds();
            await _logs.Write("Connections", guild, "Joined.");
        }

        private async Task OnGuildLeft(SocketGuild guild)
        {
            await UpdateGuilds();
            await _logs.Write("Connections", guild, "Left.");
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
    }
}