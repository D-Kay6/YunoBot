using DalFactory;
using Discord.Commands;
using Discord.WebSocket;
using IDal.Interfaces.Database;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Logic.Extensions;
using Logic.Services;

namespace Logic.Handlers
{
    public class CommandHandler : BaseHandler
    {
        private IServiceProvider _serviceProvider;
        private CommandService _service;
        private IServerSettings _settings;

        public CommandHandler(DiscordSocketClient client, IServiceProvider serviceProvider) : base(client)
        {
            _serviceProvider = serviceProvider;
            _settings = DatabaseFactory.GenerateServerSettings();
            _service = new CommandService(new CommandServiceConfig
            {
                DefaultRunMode = RunMode.Async
            });
        }

        public override async Task Initialize()
        {
            Client.MessageReceived += HandleCommandAsync;
            await _service.AddModulesAsync(Assembly.GetExecutingAssembly(), _serviceProvider);
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

                if (s.Author.Id.Equals(255453041531158538))
                {
                    var command = s.Content.Substring(argPos).ToLower();
                    switch (command)
                    {
                        case "commands":
                            var commands = "";
                            _service.Commands.Foreach(async x =>
                            {
                                commands += x.Name;
                                commands += "\n";
                            });

                            await s.Channel.SendMessageAsync(commands);
                            return;
                        case "modules":
                            var modules = "";
                            _service.Modules.Foreach(async x =>
                            {
                                modules += x.Name;
                                if (x.Parent != null) modules += $", {x.Parent}";
                                modules += "\n";
                            });
                            await s.Channel.SendMessageAsync(modules);
                            return;
                    }
                }

                LogService.Instance.Log("Commands", context.Guild, $"{context.User.Username} executed command '{context.Message}'.");
                var result = await _service.ExecuteAsync(context, argPos, _serviceProvider);
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