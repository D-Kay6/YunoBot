using Discord.Commands;
using IDal.Database;
using Logic.Services;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Group("support")]
    public class SupportModule : ModuleBase<SocketCommandContext>
    {
        private IDbLanguage _language;
        private LocalizationService _localization;

        public SupportModule(IDbLanguage language, LocalizationService localization)
        {
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
        public async Task DefaultSupport()
        {
            await ReplyAsync(_localization.GetMessage("Support default"));
        }
    }
}