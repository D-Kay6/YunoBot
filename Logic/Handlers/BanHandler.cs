using Discord.WebSocket;
using Logic.Exceptions;
using Logic.Services;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace Logic.Handlers
{
    public class BanHandler : BaseHandler
    {
        private readonly Timer _timer;
        private readonly UserService _users;

        private bool _isRunning;

        public BanHandler(DiscordShardedClient client, LogsService logs, UserService users) : base(client, logs)
        {
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

            try
            {
                await _users.CheckBans();
            }
            catch (InvalidBanException e)
            {
                await Logs.Write("Bans", e.Message, e);
            }
            catch (Exception e)
            {
                await Logs.Write("Crashes", "Could not handle tick for bans.", e);
            }

            _isRunning = false;
        }
    }
}