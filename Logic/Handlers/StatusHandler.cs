using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;

namespace Logic.Handlers
{
    public class StatusHandler : BaseHandler
    {
        private Timer _timer;
        private Random _random;

        public StatusHandler(DiscordSocketClient client) : base(client)
        {
            _timer = new Timer { Interval = TimeSpan.FromMinutes(5).TotalMilliseconds };
            _random = new Random();
        }

        public override async Task Initialize()
        {
            Client.Ready += OnReady;
            _timer.Elapsed += OnTick;
        }

        private async Task OnReady()
        {
            await RandomizeActivity();
            _timer.Start();
        }

        private async void OnTick(object sender, ElapsedEventArgs e)
        {
            await RandomizeActivity();
        }

        private async Task RandomizeActivity()
        {
            var i = _random.Next(1, 5);
            IActivity activity = null;
            //i = 10;
            switch (i)
            {
                case 1:
                    activity = new Game("Yukiteru Diary", ActivityType.Watching);
                    break;
                case 2:
                    activity = new Game($"on {this.Client.Guilds.Count} servers", ActivityType.Playing);
                    break;
                case 3:
                    activity = new Game("some music", ActivityType.Listening);
                    break;
                case 4:
                    activity = new Game("with her knife", ActivityType.Playing);
                    break;
                case 5:
                    var userCount = Client.Guilds.Sum(guild => guild.MemberCount);
                    activity = new Game($"{userCount} users", ActivityType.Watching);
                    break;
                case 10:
                    activity = new Game("Getting updates", ActivityType.Playing);
                    break;
                case 11:
                    activity = new Game("Restarting", ActivityType.Playing);
                    break;
            }
            await Client.SetActivityAsync(activity);
        }
    }
}
