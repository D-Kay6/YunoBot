using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Yuno.Data.Core.Interfaces;

namespace Yuno.Main.Commands
{
    public class CommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _service;
        private IServiceProvider _services;
        private ISerializer _serializer;

        public async Task Initialize(DiscordSocketClient client, IServiceProvider services, ISerializer serializer)
        {
            this._client = client;
            _services = services;
            this._service = new CommandService();
            this._serializer = serializer;
            await _service.AddModulesAsync(Assembly.GetExecutingAssembly());
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            if (!(s is SocketUserMessage msg)) return;
            var context = new SocketCommandContext(_client, msg);
            var argPos = 0;
            if (msg.HasStringPrefix(_serializer.Read(context.Guild.Id).CommandSettings.Prefix, ref argPos) || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
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
