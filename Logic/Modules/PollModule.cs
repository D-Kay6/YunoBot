using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Group("poll")]
    public class PollModule : ModuleBase<SocketCommandContext>
    {
        private Localization.Localization _lang;

        protected override void BeforeExecute(CommandInfo command)
        {
            _lang = new Localization.Localization(Context.Guild.Id);
            base.BeforeExecute(command);
        }

        [Command]
        public async Task DefaultPoll([Remainder] string message)
        {
            return;
        }
    }
}
