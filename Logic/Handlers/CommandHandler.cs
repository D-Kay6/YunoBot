using DalFactory;
using Discord.Commands;
using Discord.WebSocket;
using IDal.Interfaces.Database;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Logic.Services;

namespace Logic.Handlers
{
    public class CommandHandler : BaseHandler
    {
        private CommandService _service;
        private IServerSettings _settings;

        public CommandHandler(DiscordSocketClient client, IServiceProvider serviceProvider) : base(client, serviceProvider)
        {
            _settings = DatabaseFactory.GenerateServerSettings();
            _service = new CommandService(new CommandServiceConfig
            {
                DefaultRunMode = RunMode.Async
            });
        }

        public override async Task Initialize()
        {
            Client.MessageReceived += HandleCommandAsync;
            await _service.AddModulesAsync(Assembly.GetExecutingAssembly(), ServiceProvider);
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            try
            {
                if (s.Author.IsBot) return;
                if (!(s is SocketUserMessage msg)) return;
                var context = new SocketCommandContext(Client, msg);
                var prefix = _settings.GetCommandPrefix(context.Guild.Id);
                var argPos = 0;
                if (!msg.HasStringPrefix(prefix, ref argPos) && !msg.HasMentionPrefix(Client.CurrentUser, ref argPos)) return;

                LogService.Instance.Log("Commands", context.Guild, $"{context.User.Username} executed command '{context.Message}'.");
                var result = await _service.ExecuteAsync(context, argPos, ServiceProvider);
                if (result.IsSuccess) return;
                LogService.Instance.Log("Commands", context.Guild, $"Execution failed. Error code: {result.ErrorReason}.");
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
                if ((s.Channel as SocketGuildChannel).Guild.Id.Equals(264445053596991498)) return;
                LogService.Instance.Log("Crashes", $"CommandHandler crashed. Stacktrace: {e}");
            }
        }
    }
}