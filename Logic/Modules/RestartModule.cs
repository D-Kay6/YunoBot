using Discord.Commands;
using IDal.Database;
using Logic.Services;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [RequireOwner]
    public class RestartModule : ModuleBase<SocketCommandContext>
    {
        private IDbLanguage _language;
        private RestartService _service;
        private LocalizationService _localization;

        public RestartModule(IDbLanguage language, RestartService service, LocalizationService localization)
        {
            _language = language;
            _service = service;
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

        [Command("restart")]
        public async Task RestartCommand()
        {
            await Context.Message.DeleteAsync();
            await ReplyAsync(_localization.GetMessage("Restart default"));
            _service.Restart();
        }

        [Command("hardrestart")]
        public async Task RestartHardCommand()
        {
            await Context.Message.DeleteAsync();
            await ReplyAsync(_localization.GetMessage("Restart default"));
            _service.HardRestart();
        }

        [Command("shutdown")]
        public async Task ShutdownCommand()
        {
            await Context.Message.DeleteAsync();
            await ReplyAsync(_localization.GetMessage("Shutdown default"));
            _service.Shutdown();
        }
    }
}