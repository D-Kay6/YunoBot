using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Discord.WebSocket;
using IDal.Interfaces.Database;

namespace Logic.Handlers
{ 
    public class BanHandler : BaseHandler
    {
        private IDbBan _ban;
        private Timer _timer;

        public BanHandler(DiscordSocketClient client, IDbBan ban) : base(client)
        {
            _ban = ban;
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
        }

        private async void OnTick(object sender, ElapsedEventArgs e)
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
    }
}
