using Discord;
using Discord.Commands;
using IDal.Database;
using Logic.Services;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Group("prefix")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class PrefixModule : ModuleBase<SocketCommandContext>
    {
        private readonly IDbCommand _command;
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;

        public PrefixModule(IDbCommand command, IDbLanguage language, LocalizationService localization)
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
        public async Task DefaultPrefix()
        {
            await ReplyAsync(_localization.GetMessage("Prefix default", await _command.GetPrefix(Context.Guild.Id), Context.Guild.Name));
        }

        [Command("set")]
        public async Task PrefixSet([Remainder] string message)
        {
            await _command.SetPrefix(Context.Guild.Id, message);
            await ReplyAsync(_localization.GetMessage("Prefix set", message, Context.Guild.Name));
        }
    }
}