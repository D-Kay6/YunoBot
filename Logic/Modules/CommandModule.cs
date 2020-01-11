using Discord;
using Discord.Commands;
using IDal.Database;
using Logic.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Group("Command")]
    public class CommandModule : ModuleBase<SocketCommandContext>
    {
        [Group("prefix")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public class CommandPrefixModule : ModuleBase<SocketCommandContext>
        {
            private readonly IDbCommand _command;
            private readonly IDbLanguage _language;
            private readonly LocalizationService _localization;

            public CommandPrefixModule(IDbCommand command, IDbLanguage language, LocalizationService localization)
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
            private IDbCommand _command;
            private IDbLanguage _language;
            private LocalizationService _localization;

            public CommandCustomModule(IDbCommand command, IDbLanguage language, LocalizationService localization)
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
                await ReplyAsync(_localization.GetMessage("Command custom default", (await _command.GetCustomCommands(Context.Guild.Id)).Count));
            }

            [Command("list")]
            public async Task CommandCustomList()
            {
                var prefix = await _command.GetPrefix(Context.Guild.Id);
                var commands = await _command.GetCustomCommands(Context.Guild.Id);
                var commandMsg = string.Join("\n", commands.Select(x => $"{prefix}{x.Command}"));

                await ReplyAsync(_localization.GetMessage("Command custom list", commandMsg));
            }

            [Command("Add")]
            [RequireUserPermission(GuildPermission.Administrator)]
            public async Task CommandCustomAdd(string command, [Remainder] string response)
            {
                var customCommand = await _command.GetCustomCommand(Context.Guild.Id, command);
                if (customCommand != null)
                {
                    await ReplyAsync(_localization.GetMessage("Command custom exists"));
                    return;
                }

                await _command.AddCustomCommand(Context.Guild.Id, command, response);
                await ReplyAsync(_localization.GetMessage("Command custom added", command));
            }

            [Command("Remove")]
            [RequireUserPermission(GuildPermission.Administrator)]
            public async Task CommandCustomRemove([Remainder] string command)
            {
                var customCommand = await _command.GetCustomCommand(Context.Guild.Id, command);
                if (customCommand == null)
                {
                    await ReplyAsync(_localization.GetMessage("Command custom none", command));
                    return;
                }

                await _command.RemoveCustomCommand(customCommand);
                await ReplyAsync(_localization.GetMessage("Command custom removed", command));
            }
        }
    }
}
