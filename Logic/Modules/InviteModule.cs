using Discord.Commands;
using System.Threading.Tasks;
using Logic.Extensions;

namespace Logic.Modules
{
    [Group("Invite")]
    public class InviteModule : ModuleBase<SocketCommandContext>
    {
        private Localization.Localization _lang;

        protected override void BeforeExecute(CommandInfo command)
        {
            _lang = new Localization.Localization(Context.Guild.Id);
            base.BeforeExecute(command);
        }

        [Command]
        public async Task DefaultInvite()
        {
            await Context.User.SendDM(_lang.GetMessage("Invite default"));
        }
    }
}
