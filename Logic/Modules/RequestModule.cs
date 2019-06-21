using Discord.Commands;
using Logic.Handlers;
using System.Threading.Tasks;

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
            LogsHandler.Instance.Log("Features", message);
            await ReplyAsync(_lang.GetMessage("Request feature"));
        }

        [Command("change")]
        public async Task RequestChange([Remainder] string message)
        {
            LogsHandler.Instance.Log("Changes", message);
            await ReplyAsync(_lang.GetMessage("Request change"));
        }
    }
}
