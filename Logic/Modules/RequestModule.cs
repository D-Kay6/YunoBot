using Discord.Commands;
using Logic.Handlers;
using System.Threading.Tasks;
using IDal.Interfaces.Database;
using Logic.Services;

namespace Logic.Modules
{
    [Group("request")]
    public class RequestModule : ModuleBase<SocketCommandContext>
    {
        private IDbLanguage _language;
        private Localization.Localization _lang;

        public RequestModule(IDbLanguage language)
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
        public async Task DefaultRequest()
        {
            await ReplyAsync(_lang.GetMessage("Request default"));
        }

        [Command("feature")]
        public async Task RequestFeature([Remainder] string message)
        {
            LogService.Instance.Log("Features", message);
            await ReplyAsync(_lang.GetMessage("Request feature"));
        }

        [Command("change")]
        public async Task RequestChange([Remainder] string message)
        {
            LogService.Instance.Log("Changes", message);
            await ReplyAsync(_lang.GetMessage("Request change"));
        }
    }
}
