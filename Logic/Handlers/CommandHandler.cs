using DalFactory;
using Discord.Commands;
using Discord.WebSocket;
using IDal.Interfaces.Database;
using System;
using System.Reflection;
using System.Threading.Tasks;

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
            try
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
                var lang = new Localization.Localization(context.Guild.Id);
                switch (result.Error)
                {
                    case CommandError.UnmetPrecondition:
                        await context.Channel.SendMessageAsync(lang.GetMessage("Command invalid permissions"));
                        break;
                    case CommandError.BadArgCount:
                        await context.Channel.SendMessageAsync(lang.GetMessage("Command invalid arguments", prefix));
                        break;
                    default:
                        await context.Channel.SendMessageAsync(lang.GetMessage("Command invalid", prefix));
                        break;
                }
            }
            catch (Exception e)
            {
                LogsHandler.Instance.Log("Crashes", $"CommandHandler crashed. Stacktrace: {e}");
            }
        }
    }
}