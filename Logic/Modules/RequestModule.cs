using Discord.Commands;
using IDal.Database;
using Logic.Services;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Group("request")]
    public class RequestModule : ModuleBase<ShardedCommandContext>
    {
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;
        private readonly LogsService _logs;

        public RequestModule(IDbLanguage language, LocalizationService localization, LogsService logs)
        {
            _language = language;
            _localization = localization;
            _logs = logs;
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
        public async Task DefaultRequest()
        {
            await ReplyAsync(_localization.GetMessage("Request default"));
        }

        [Command("feature")]
        public async Task RequestFeature([Remainder] string message)
        {
            await _logs.Write("Features", message);
            await ReplyAsync(_localization.GetMessage("Request feature"));
        }

        [Command("change")]
        public async Task RequestChange([Remainder] string message)
        {
            await _logs.Write("Changes", message);
            await ReplyAsync(_localization.GetMessage("Request change"));
        }
    }
}