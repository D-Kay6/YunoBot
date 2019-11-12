using Discord.WebSocket;
using Logic.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace Logic.Handlers
{
    public class UpdateHandler : BaseHandler
    {
        private Timer _timer;
        private UpdateService _service;

        public UpdateHandler(DiscordSocketClient client, UpdateService updateService) : base(client)
        {
            _timer = new Timer { Interval = TimeSpan.FromMinutes(1).TotalMilliseconds };
            _service = updateService;
        }

        public override async Task Initialize()
        {
            _timer.Elapsed += OnTick;
            _timer.Start();
        }

        private async void OnTick(object sender, ElapsedEventArgs e)
        {
            //if (!await _service.HasUpdate()) return;
            //await _service.Update();
        }
    }
}
