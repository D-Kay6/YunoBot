using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using Logic.Extensions;

namespace Logic.Modules
{
    [Group("kill")]
    public class KillModule : ModuleBase<SocketCommandContext>
    {
        private Localization.Localization _lang;

        protected override void BeforeExecute(CommandInfo command)
        {
            _lang = new Localization.Localization(Context.Guild.Id);
            base.BeforeExecute(command);
        }

        [Priority(-1)]
        [Command]
        public async Task DefaultCommand([Remainder] string name)
        {
            await ReplyAsync(_lang.GetMessage("Invalid user", name));
        }

        [Command]
        public async Task DefaultCommand(SocketGuildUser user)
        {
            if (user == null) return;
            switch (user.Id)
            {
                case 255453041531158538:
                    await ReplyAsync(_lang.GetMessage("Kill creator", user.Nickname()));
                    break;
                case 286972781273546762:
                    await ReplyAsync(_lang.GetMessage("Kill self"));
                    break;
                default:
                    await Context.Channel.SendFileAsync(ImageExtensions.GetImagePath("GasaiYuno.gif"), _lang.GetMessage("Kill default", user.Mention));
                    break;
            }
        }
    }
}