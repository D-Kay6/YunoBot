using Discord.Commands;
using Discord.WebSocket;
using IDal.Interfaces.Database;
using Logic.Services;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Logic.Handlers
{
    public class CommandHandler : BaseHandler
    {
        private IDbLanguage _language;
        private IDbCommand _command;
        private IServiceProvider _serviceProvider;
        private CommandService _service;

        public CommandHandler(DiscordSocketClient client, IDbLanguage language, IDbCommand command, IServiceProvider serviceProvider) : base(client)
        {
            _language = language;
            _command = command;
            _serviceProvider = serviceProvider;
            _service = new CommandService(new CommandServiceConfig
            {
                DefaultRunMode = RunMode.Async
            });
        }

        public override async Task Initialize()
        {
            await _service.AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider);
            Client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            try
            {
                if (s.Author.IsBot) return;
                if (!(s is SocketUserMessage msg)) return;
                var context = new SocketCommandContext(Client, msg);
                var prefix = await _command.GetPrefix(context.Guild.Id);
                var argPos = 0;
                if (!msg.HasStringPrefix(prefix, ref argPos) && !msg.HasMentionPrefix(Client.CurrentUser, ref argPos)) return;

                LogService.Instance.Log("Commands", context.Guild, $"{context.User.Username} executed command '{context.Message}'.");
                var result = await _service.ExecuteAsync(context, argPos, _serviceProvider);
                if (result.IsSuccess) return;
                LogService.Instance.Log("Commands", context.Guild, $"Execution failed. Error code: {result.ErrorReason}.");
                var lang = new Localization.Localization(await _language.GetLanguage(context.Guild.Id));
                switch (result.Error)
                {
                    case CommandError.UnmetPrecondition:
                        await context.Channel.SendMessageAsync(lang.GetMessage("Command invalid permissions"));
                        break;
                    case CommandError.BadArgCount:
                        await context.Channel.SendMessageAsync(lang.GetMessage("Command invalid arguments", prefix));
                        break;
                    default:
                        if (await SendCustomCommand(context, argPos)) return;
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

        private async Task<bool> SendCustomCommand(SocketCommandContext context, int argPos)
        {
            var message = context.Message.Content.Substring(argPos);
            var command = await _command.GetCustomCommand(context.Guild.Id, message);
            if (command == null) return false;
            await context.Channel.SendMessageAsync(command.Response);
            return true;
        }
    }
}