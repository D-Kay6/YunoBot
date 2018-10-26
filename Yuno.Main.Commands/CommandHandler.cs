using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace Yuno.Main.Commands
{
    public class CommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _service;
        private IServiceProvider _services;
        private string _prefix;

        public async Task Initialize(DiscordSocketClient client, IServiceProvider services, string prefix)
        {
            this._client = client;
            _services = services;
            this._service = new CommandService();
            this._prefix = prefix;
            await _service.AddModulesAsync(Assembly.GetExecutingAssembly());
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null) return;
            var context = new SocketCommandContext(_client, msg);
            var argPos = 0;
            if (msg.HasStringPrefix(_prefix, ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _service.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    Console.WriteLine(result.ErrorReason);
                }
            }
        }
    }
}
