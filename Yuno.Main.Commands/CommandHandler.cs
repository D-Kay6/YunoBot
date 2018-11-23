using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Yuno.Logic;

namespace Yuno.Main.Commands
{
    public class CommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _service;
        private IServiceProvider _services;

        public async Task Initialize(DiscordSocketClient client, IServiceProvider services)
        {
            this._client = client;
            _services = services;
            this._service = new CommandService();
            await _service.AddModulesAsync(Assembly.GetExecutingAssembly());
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            if (!(s is SocketUserMessage msg)) return;
            var context = new SocketCommandContext(_client, msg);
            var prefix = CommandSettings.Load(context.Guild.Id).Prefix;
            var argPos = 0;
            if (!msg.HasStringPrefix(prefix, ref argPos) && !msg.HasMentionPrefix(_client.CurrentUser, ref argPos)) return;
            ËxecuteCommandAsync(context, prefix, argPos);
        }

        private async Task ËxecuteCommandAsync(SocketCommandContext context, string prefix, int argPos)
        {
            var result = await _service.ExecuteAsync(context, argPos, _services);
            if (result.IsSuccess) return;
            Console.WriteLine(result.ErrorReason);
            switch (result.Error)
            {
                case CommandError.UnmetPrecondition:
                    await context.Channel.SendMessageAsync($"I'm unable to do that. You lack the required permissions for this.");
                    break;
                case CommandError.BadArgCount:
                    await context.Channel.SendMessageAsync($"Ehmm... I think you made a mistake somewhere. Try using {prefix}help if you forgot the syntax.");
                    break;
                default:
                    await context.Channel.SendMessageAsync($"Sorry, I don't know what to do with that.");
                    break;
            }
        }
    }
}
