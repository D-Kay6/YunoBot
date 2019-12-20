using DalFactory;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using IDal.Interfaces.Database;

namespace Logic.Modules
{
    [Group("prefix")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class PrefixModule : ModuleBase<SocketCommandContext>
    {
        private IDbCommand _command;
        private IDbLanguage _language;
        private Localization.Localization _lang;

        public PrefixModule(IDbCommand command, IDbLanguage language)
        {
            _command = command;
            _language = language;
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            Task.WaitAll(LoadLanguage());
            base.BeforeExecute(command);
        }

        private async Task LoadLanguage()
        {
            _lang = new Localization.Localization(await _language.GetLanguage(Context.Guild.Id));
        }

        [Command]
        public async Task DefaultPrefix()
        {
            await ReplyAsync(_lang.GetMessage("Prefix default", await _command.GetPrefix(Context.Guild.Id), Context.Guild.Name));
        }

        [Command("set")]
        public async Task PrefixSet([Remainder] string message)
        {
            await _command.SetPrefix(Context.Guild.Id, message);
            await ReplyAsync(_lang.GetMessage("Prefix set", message, Context.Guild.Name));
        }
    }
}