using Logic.Exceptions;

namespace Logic.Handlers
{
    using System;
    using System.Threading.Tasks;
    using System.Timers;
    using Discord.WebSocket;
    using Services;

    public class BanHandler : BaseHandler
    {
        private readonly UserService _users;
        private readonly LogsService _logs;

        private readonly Timer _timer;

        private bool _isRunning;

        public BanHandler(DiscordSocketClient client, UserService users, LogsService logs) : base(client)
        {
            _users = users;
            _logs = logs;
            _timer = new Timer(10 * 1000);
        }

        public override Task Initialize()
        {
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
                await _logs.Write("Bans", e.Message);
            }
            catch (Exception e)
            {
                await _logs.Write("Crashes", $"Could not handle tick for bans. {e.Message}, {e.StackTrace}");
            }

            _isRunning = false;
        }
    }
}