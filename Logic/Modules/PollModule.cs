using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using IDal.Interfaces.Database;

namespace Logic.Modules
{
    [Group("poll")]
    public class PollModule : ModuleBase<SocketCommandContext>
    {
        private IDbLanguage _language;
        private Localization.Localization _lang;

        public PollModule(IDbLanguage language)
        {
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
        public async Task DefaultPoll([Remainder] string message)
        {
            return;
        }
    }
}
