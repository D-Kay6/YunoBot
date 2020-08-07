using Discord.WebSocket;
using DiscordBotsList.Api;
using Logic.Exceptions;
using Logic.Services;
using System;
using System.Threading.Tasks;

namespace Logic.Handlers
{
    public class DblHandler : BaseHandler
    {
        private readonly ConfigurationService _configuration;

        private bool _isRunning;

        public DblHandler(DiscordShardedClient client, LogsService logs, ConfigurationService configuration) : base(client, logs)
        {
            _configuration = configuration;
        }

        public AuthDiscordBotListApi DblApi { get; private set; }

        public override async Task Initialize()
        {
            await base.Initialize();
            try
            {
                var token = await _configuration.GetDblToken();
                var clientId = await _configuration.GetClientId();
                DblApi = new AuthDiscordBotListApi(clientId, token);
            }
            catch (InvalidTokenException e)
            {
                Console.WriteLine(e.Message);
                return;
            }
            catch (InvalidIdException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            Client.JoinedGuild += OnGuildJoined;
            Client.LeftGuild += OnGuildLeft;
        }

        protected override async Task Ready(DiscordSocketClient client)
        {
            await base.Ready(client);
            await UpdateGuilds();
        }

        private async Task OnGuildJoined(SocketGuild guild)
        {
            await UpdateGuilds();
            await Logs.Write("Connections", "Joined.", guild);
        }

        private async Task OnGuildLeft(SocketGuild guild)
        {
            await UpdateGuilds();
            await Logs.Write("Connections", "Left.", guild);
        }

        private async Task UpdateGuilds()
        {
            if (_isRunning) return;
#if DEBUG
            return;
#endif
            try
            {
                _isRunning = true;
                var me = await DblApi.GetMeAsync();
                await me.UpdateStatsAsync(Client.Guilds.Count);
                await DblApi.UpdateStats(Client.Guilds.Count);
                Console.WriteLine($"Updating guilds to {Client.Guilds.Count}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Could not update guild count. {e.Message}");
            }
            finally
            {
                _isRunning = false;
            }
        }
    }
}