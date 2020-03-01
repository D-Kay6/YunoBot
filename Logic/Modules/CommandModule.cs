using Discord;
using Discord.Commands;
using IDal.Database;
using Logic.Exceptions;
using Logic.Services;
using System.Linq;
using System.Threading.Tasks;
using CommandService = Logic.Services.CommandService;

namespace Logic.Modules
{
    [Group("Command")]
    public class CommandModule : ModuleBase<SocketCommandContext>
    {
        [Group("prefix")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public class CommandPrefixModule : ModuleBase<SocketCommandContext>
        {
            private readonly CommandService _command;
            private readonly IDbLanguage _language;
            private readonly LocalizationService _localization;

            public CommandPrefixModule(CommandService command, IDbLanguage language, LocalizationService localization)
            {
                _command = command;
                _language = language;
                _localization = localization;
            }

            protected override void BeforeExecute(CommandInfo command)
            {
                Task.WaitAll(Prepare());
                base.BeforeExecute(command);
            }

            private async Task Prepare()
            {
                await _localization.Load(await _language.GetLanguage(Context.Guild.Id));
            }

            [Command]
            public async Task CommandPrefixDefault()
            {
                await ReplyAsync(_localization.GetMessage("Command prefix default", await _command.GetPrefix(Context.Guild.Id), Context.Guild.Name));
            }

            [Command("set")]
            public async Task CommandPrefixSet([Remainder] string message)
            {
                await _command.SetPrefix(Context.Guild.Id, message);
                await ReplyAsync(_localization.GetMessage("Command prefix set", message, Context.Guild.Name));
            }
        }

        [Group("custom")]
        public class CommandCustomModule : ModuleBase<SocketCommandContext>
        {
            private CommandService _command;
            private IDbLanguage _language;
            private LocalizationService _localization;

            public CommandCustomModule(CommandService command, IDbLanguage language, LocalizationService localization)
            {
                _command = command;
                _language = language;
                _localization = localization;
            }

            protected override void BeforeExecute(CommandInfo command)
            {
                Task.WaitAll(Prepare());
                base.BeforeExecute(command);
            }

            private async Task Prepare()
            {
                await _localization.Load(await _language.GetLanguage(Context.Guild.Id));
            }

            [Command]
            public async Task CommandCustomDefault()
            {
                await ReplyAsync(_localization.GetMessage("Command custom default", (await _command.GetAllCustom(Context.Guild.Id)).Count));
            }

            [Command("list")]
            public async Task CommandCustomList()
            {
                var prefix = await _command.GetPrefix(Context.Guild.Id);
                var commands = await _command.GetAllCustom(Context.Guild.Id);
                var commandMsg = string.Join("\n", commands.Select(x => $"{prefix}{x.Command}"));

                await ReplyAsync(_localization.GetMessage("Command custom list", commandMsg));
            }

            [Command("Add")]
            [RequireUserPermission(GuildPermission.Administrator)]
            public async Task CommandCustomAdd(string command, [Remainder] string response)
            {
                try
                {
                    await _command.AddCustomCommand(Context.Guild.Id, command, response);
                }
                catch (CommandExistsException)
                {
                    await ReplyAsync(_localization.GetMessage("Command custom exists"));
                    return;
                }

                await ReplyAsync(_localization.GetMessage("Command custom added", command));
            }

            [Command("Remove")]
            [RequireUserPermission(GuildPermission.Administrator)]
            public async Task CommandCustomRemove([Remainder] string command)
            {
                try
                {
                    await _command.RemoveCustomCommand(Context.Guild.Id, command);
                }
                catch (InvalidCommandException)
                {
                    await ReplyAsync(_localization.GetMessage("Command custom none", command));
                    return;
                }

                await ReplyAsync(_localization.GetMessage("Command custom removed", command));
            }
        }
    }
}