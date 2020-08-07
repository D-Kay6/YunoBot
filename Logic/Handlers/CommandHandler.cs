using Discord.Commands;
using Discord.WebSocket;
using IDal.Database;
using Logic.Exceptions;
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
        private readonly ServerService _server;
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;
        private readonly IServiceProvider _serviceProvider;

        private readonly Discord.Commands.CommandService _dcService;

        public CommandHandler(DiscordShardedClient client, LogsService logs, CommandService commandService, ServerService server, IDbLanguage language, LocalizationService localization, IServiceProvider serviceProvider) : base(client, logs)
        {
            _commandService = commandService;
            _server = server;
            _language = language;
            _localization = localization;
            _serviceProvider = serviceProvider;
            _dcService = new Discord.Commands.CommandService(new CommandServiceConfig
            {
                DefaultRunMode = RunMode.Async
            });
        }

        public override async Task Initialize()
        {
            await base.Initialize();
            await _dcService.AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider);
            Client.MessageReceived += HandleCommandAsync;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            try
            {
                if (s.Author.IsBot) return;
                if (!(s is SocketUserMessage msg)) return;
                var context = new ShardedCommandContext(Client, msg);
                var prefix = "/";
                try
                {
                    prefix = await _commandService.GetPrefix(context.Guild.Id);
                }
                catch (UnknownServerException e)
                {
                    await _server.Update(context.Guild);
                }
                var argPos = 0;
                if (!msg.HasStringPrefix(prefix, ref argPos) &&
                    !msg.HasMentionPrefix(Client.CurrentUser, ref argPos)) return;

#if DEBUG
                if (!s.Author.Id.Equals(255453041531158538))
                {
                    await context.Channel.SendMessageAsync("Sorry, I cannot do that right now. I'm under development");
                    return;
                }
#endif
                await Logs.Write("Commands", $"{context.User.Username} executed command '{context.Message}'.", context.Guild);
                var result = await _dcService.ExecuteAsync(context, argPos, _serviceProvider);
                if (result.IsSuccess) return;
                await Logs.Write("Commands", $"Execution failed. Error code: {result.ErrorReason}.", context.Guild);
                await _localization.Load(await _language.GetLanguage(context.Guild.Id));
                switch (result.Error)
                {
                    case CommandError.UnmetPrecondition:
                        await context.Channel.SendMessageAsync(_localization.GetMessage("Command invalid permissions"));
                        break;
                    case CommandError.BadArgCount:
                        await context.Channel.SendMessageAsync(_localization.GetMessage("Command invalid arguments",
                            prefix));
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
                await Logs.Write("Crashes", "CommandHandler crashed.", e);
            }
        }

        private async Task<bool> SendCustomCommand(SocketCommandContext context, int argPos)
        {
            var command = context.Message.Content.Substring(argPos);
            try
            {
                var response = await _commandService.GetResponse(context.Guild.Id, command);
                await context.Channel.SendMessageAsync(response);
            }
            catch (InvalidCommandException)
            {
                return false;
            }

            return true;
        }
    }
}