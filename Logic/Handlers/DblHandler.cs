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
        private readonly LogsService _logs;

        private bool _isRunning;

        public AuthDiscordBotListApi DblApi { get; private set; }

        public DblHandler(DiscordSocketClient client, ConfigurationService configuration, LogsService logs) : base(client)
        {
            _configuration = configuration;
            _logs = logs;
        }

        public override async Task Initialize()
        {
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

            Client.Ready += OnReady;
            Client.JoinedGuild += OnGuildJoined;
            Client.LeftGuild += OnGuildLeft;
        }

        private async Task OnReady()
        {
            await UpdateGuilds();
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
            if (_isRunning) return;

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