using Discord.WebSocket;
using IDal.Database;
using System;
using System.Threading.Tasks;
using System.Timers;
using Logic.Services;

namespace Logic.Handlers
{
    public class BanHandler : BaseHandler
    {
        private IDbBan _ban;
        private LogsService _logs;

        private Timer _timer;
        private bool _isRunning;

        public BanHandler(DiscordSocketClient client, IDbBan ban, LogsService logs) : base(client)
        {
            _ban = ban;
            _logs = logs;
            _timer = new Timer(10 * 1000);
        }

        public override async Task Initialize()
        {
            Client.Ready += OnReady;
            _timer.Elapsed += OnTick;
        }

        private async Task OnReady()
        {
            _timer.Start();
            FinishLoading();
        }

        private async void OnTick(object sender, ElapsedEventArgs e)
        {
            if (_isRunning) return;

            _isRunning = true;
            try
            {
                var bans = await _ban.GetBans();
                foreach (var ban in bans)
                {
                    var server = Client.GetGuild(ban.ServerId);
                    if (server == null) continue;
                    await server.RemoveBanAsync(ban.UserId);
                    await _ban.RemoveBan(ban);
                }
            }
            catch (Exception ex)
            {
                await _logs.Write("Crashes", $"Could not handle tick for bans. {ex.Message}, {ex.StackTrace}");
            }
            _isRunning = false;
        }
    }
}
