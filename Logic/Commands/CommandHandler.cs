using Discord.Commands;
using Discord.WebSocket;
using ILogic.Interfaces;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Logic.Commands
{
    public class CommandHandler : IHandler
    {
        private DiscordSocketClient _client;
        private CommandService _service;
        private IConfig _config;

        public async Task Initialize(DiscordSocketClient client, IConfig config)
        {
            this._client = client;
            this._service = new CommandService();
            this._config = config;
            await _service.AddModulesAsync(Assembly.GetExecutingAssembly());
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            var msg = s as SocketUserMessage;
            if (msg == null) return;
            var context = new SocketCommandContext(_client, msg);
            var argPos = 0;
            if (msg.HasStringPrefix(_config.GetConfig().Prefix, ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _service.ExecuteAsync(context, argPos);
                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                {
                    Console.WriteLine(result.ErrorReason);
                }
            }
        }
    }
}
