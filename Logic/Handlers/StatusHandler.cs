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
        private readonly Timer _timer;
        private readonly Random _random;

        public StatusHandler(DiscordSocketClient client) : base(client)
        {
            _timer = new Timer { Interval = TimeSpan.FromMinutes(5).TotalMilliseconds };
            _random = new Random();
        }

        public override async Task Initialize()
        {
            _timer.Elapsed += OnTick;
            _timer.Start();

            await RandomizeActivity();
        }

        private async void OnTick(object sender, ElapsedEventArgs e)
        {
            await RandomizeActivity();
        }

        private async Task RandomizeActivity()
        {
            var i = _random.Next(1, 5);
            IActivity activity = null;
#if DEBUG
            i = 10;
#endif
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
                    await Client.SetStatusAsync(UserStatus.DoNotDisturb);
                    activity = new Game("new updates", ActivityType.Watching);
                    break;
            }
            await Client.SetActivityAsync(activity);
        }
    }
}
