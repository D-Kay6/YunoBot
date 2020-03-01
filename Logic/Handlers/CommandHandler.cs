using Discord.Commands;
using Discord.WebSocket;
using IDal.Database;
using Logic.Services;
using System;
using System.Reflection;
using System.Threading.Tasks;
using CommandService = Logic.Services.CommandService;

namespace Logic.Handlers
{
    public class CommandHandler : BaseHandler
    {
        private readonly CommandService _commandService;
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;
        private readonly LogsService _logs;
        private readonly IServiceProvider _serviceProvider;

        private readonly Discord.Commands.CommandService _dcService;

        public CommandHandler(DiscordSocketClient client, CommandService commandService, IDbLanguage language, LocalizationService localization, LogsService logs, IServiceProvider serviceProvider) : base(client)
        {
            _commandService = commandService;
            _language = language;
            _localization = localization;
            _logs = logs;
            _serviceProvider = serviceProvider;
            _dcService = new Discord.Commands.CommandService(new CommandServiceConfig
            {
                DefaultRunMode = RunMode.Async
            });
        }

        public override async Task Initialize()
        {
            await _dcService.AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider);
            Client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            try
            {
                if (s.Author.IsBot) return;
                if (!(s is SocketUserMessage msg)) return;
                var context = new SocketCommandContext(Client, msg);
                var prefix = await _commandService.GetPrefix(context.Guild.Id);
                var argPos = 0;
                if (!msg.HasStringPrefix(prefix, ref argPos) && !msg.HasMentionPrefix(Client.CurrentUser, ref argPos)) return;

#if DEBUG
                if (!s.Author.Id.Equals(255453041531158538))
                {
                    await context.Channel.SendMessageAsync("Sorry, I cannot do that right now. I'm under development");
                    return;
                }
#endif
                await _logs.Write("Commands", context.Guild, $"{context.User.Username} executed command '{context.Message}'.");
                var result = await _dcService.ExecuteAsync(context, argPos, _serviceProvider);
                if (result.IsSuccess) return;
                await _logs.Write("Commands", context.Guild, $"Execution failed. Error code: {result.ErrorReason}.");
                await _localization.Load(await _language.GetLanguage(context.Guild.Id));
                switch (result.Error)
                {
                    case CommandError.UnmetPrecondition:
                        await context.Channel.SendMessageAsync(_localization.GetMessage("Command invalid permissions"));
                        break;
                    case CommandError.BadArgCount:
                        await context.Channel.SendMessageAsync(_localization.GetMessage("Command invalid arguments", prefix));
                        break;
                    default:
                        if (await SendCustomCommand(context, argPos)) return;
                        await context.Channel.SendMessageAsync(_localization.GetMessage("Command invalid", prefix));
                        break;
                }
            }
            catch (Exception e)
            {
                if ((s.Channel as SocketGuildChannel).Guild.Id.Equals(264445053596991498)) return;
                await _logs.Write("Crashes", $"CommandHandler crashed. Stacktrace: {e}");
            }
        }

        private async Task<bool> SendCustomCommand(SocketCommandContext context, int argPos)
        {
            var command = context.Message.Content.Substring(argPos);
            var response = await _commandService.GetResponse(context.Guild.Id, command);
            if (response == null) return false;
            await context.Channel.SendMessageAsync(response);
            return true;
        }
    }
}