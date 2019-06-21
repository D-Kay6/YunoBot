using Discord.Commands;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Group("support")]
    public class SupportModule : ModuleBase<SocketCommandContext>
    {
        private Localization.Localization _lang;

        protected override void BeforeExecute(CommandInfo command)
        {
            _lang = new Localization.Localization(Context.Guild.Id);
            base.BeforeExecute(command);
        }

        [Command]
        public async Task DefaultSupport()
        {
            await ReplyAsync(_lang.GetMessage("Support default"));
        }
    }
}