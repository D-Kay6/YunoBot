using Discord.Commands;
using Logic.Handlers;
using System.Threading.Tasks;
using IDal.Interfaces.Database;
using Logic.Services;

namespace Logic.Modules
{
    [RequireOwner]
    public class RestartModule : ModuleBase<SocketCommandContext>
    {
        private IDbLanguage _language;
        private RestartService _service;
        private Localization.Localization _lang;

        public RestartModule(IDbLanguage language, RestartService service)
        {
            _language = language;
            _service = service;
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

        [Command("restart")]
        public async Task RestartCommand()
        {
            await Context.Message.DeleteAsync();
            await ReplyAsync(_lang.GetMessage("Restart default"));
            _service.Restart();
        }

        [Command("hardrestart")]
        public async Task RestartHardCommand()
        {
            await Context.Message.DeleteAsync();
            await ReplyAsync(_lang.GetMessage("Restart default"));
            _service.HardRestart();
        }

        [Command("shutdown")]
        public async Task ShutdownCommand()
        {
            await Context.Message.DeleteAsync();
            await ReplyAsync(_lang.GetMessage("Shutdown default"));
            _service.Shutdown();
        }
    }
}