using Discord.Commands;
using Logic.Handlers;
using System.Threading.Tasks;
using Logic.Services;

namespace Logic.Modules
{
    [Group("request")]
    public class RequestModule : ModuleBase<SocketCommandContext>
    {
        private Localization.Localization _lang;

        protected override void BeforeExecute(CommandInfo command)
        {
            _lang = new Localization.Localization(Context.Guild.Id);
            base.BeforeExecute(command);
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
