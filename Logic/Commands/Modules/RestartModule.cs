using Discord.Commands;
using IDal.Database;
using Logic.Services;
using System.Threading.Tasks;

namespace Logic.Commands.Modules
{
    [RequireOwner]
    public class RestartModule : ModuleBase<ShardedCommandContext>
    {
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;
        private readonly RestartService _service;

        public RestartModule(RestartService service, IDbLanguage language, LocalizationService localization)
        {
            _service = service;
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

        [Command("restart")]
        public async Task RestartCommand()
        {
            await ReplyAsync(_localization.GetMessage("Restart default"));
            _service.Restart();
        }

        [Command("hardrestart")]
        public async Task RestartHardCommand()
        {
            await ReplyAsync(_localization.GetMessage("Restart default"));
            _service.HardRestart();
        }

        [Command("shutdown")]
        public async Task ShutdownCommand()
        {
            await ReplyAsync(_localization.GetMessage("Shutdown default"));
            _service.Shutdown();
        }
    }
}