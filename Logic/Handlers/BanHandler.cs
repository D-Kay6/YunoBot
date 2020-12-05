using Discord.WebSocket;
using Logic.Services;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace Logic.Handlers
{
    public class BanHandler : BaseHandler
    {
        private readonly Timer _timer;
        private readonly ServerService _servers;
        private readonly UserService _users;

        private bool _isRunning;

        public BanHandler(DiscordShardedClient client, LogsService logs, ServerService servers, UserService users) : base(client, logs)
        {
            _servers = servers;
            _users = users;
            _timer = new Timer(10 * 1000);
        }

        public override Task Initialize()
        {
            base.Initialize();
            _timer.Elapsed += OnTick;
            _timer.Start();
            return Task.CompletedTask;
        }

        private async void OnTick(object sender, ElapsedEventArgs eventArgs)
        {
            if (_isRunning) return;
            _isRunning = true;

            var bans = await _servers.Bans();
            foreach (var ban in bans)
            {
                var server = Client.GetGuild(ban.ServerId);
                if (server == null) continue;
                try
                {
                    await server.RemoveBanAsync(ban.UserId);
                }
                catch (Exception e)
                {
                    if (!e.Message.Contains("404: NotFound", StringComparison.OrdinalIgnoreCase))
                    {
                        await Logs.Write("Crashes", "Could not handle tick for bans.", e);
                        continue;
                    }
                }

                await _users.Unban(ban);
            }

            _isRunning = false;
        }
    }
}