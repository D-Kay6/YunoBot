using Discord.Commands;
using System.Threading.Tasks;
using IDal.Interfaces.Database;
using Logic.Extensions;

namespace Logic.Modules
{
    [Group("Invite")]
    public class InviteModule : ModuleBase<SocketCommandContext>
    {
        private IDbLanguage _language;
        private Localization.Localization _lang;

        public InviteModule(IDbLanguage language)
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
        public async Task DefaultInvite()
        {
            await Context.User.SendDM(_lang.GetMessage("Invite default"));
        }
    }
}
