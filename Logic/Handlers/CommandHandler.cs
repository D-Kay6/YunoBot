using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Reflection;
using System.Threading.Tasks;
using DalFactory;
using IDal.Interfaces.Database;

namespace Logic.Handlers
{
    public class CommandHandler
    {
        private DiscordSocketClient _client;
        private CommandService _service;
        private IServiceProvider _services;
        private IServerSettings _settings;

        public async Task Initialize(DiscordSocketClient client, IServiceProvider services)
        {
            _client = client;
            _services = services;
            _settings = DatabaseFactory.GenerateServerSettings();
            _service = new CommandService(new CommandServiceConfig
            {
                DefaultRunMode = RunMode.Async
            });
            await _service.AddModulesAsync(Assembly.GetExecutingAssembly(), _services);
            _client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            if (s.Author.IsBot) return;
            if (!(s is SocketUserMessage msg)) return;
            var context = new SocketCommandContext(_client, msg);
            var prefix = _settings.GetCommandPrefix(context.Guild.Id);
            var argPos = 0;
            if (!msg.HasStringPrefix(prefix, ref argPos) && !msg.HasMentionPrefix(_client.CurrentUser, ref argPos)) return;

            LogsHandler.Instance.Log("Commands", context.Guild, $"{context.User.Username} executed command '{context.Message}'.");
            var result = await _service.ExecuteAsync(context, argPos, _services);
            if (result.IsSuccess) return;
            LogsHandler.Instance.Log("Commands", context.Guild, $"Execution failed. Error code: {result.ErrorReason}.");
            switch (result.Error)
            {
                case CommandError.UnmetPrecondition:
                    await context.Channel.SendMessageAsync(
                        "I\'m unable to do that. You lack the required permissions for this.");
                    break;
                case CommandError.BadArgCount:
                    await context.Channel.SendMessageAsync(
                        $"Ehmm... I think you made a mistake somewhere. Try using {prefix}help if you forgot the syntax.");
                    break;
                default:
                    await context.Channel.SendMessageAsync(
                        $"Sorry, I don't know what to do with that. Use {prefix}help if you need a list of my commands.");
                    break;
            }
        }
    }
}