using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using IDal.Interfaces.Database;
using Logic.Extensions;

namespace Logic.Modules
{
    [Group("kill")]
    public class KillModule : ModuleBase<SocketCommandContext>
    {
        private IDbLanguage _language;
        private Localization.Localization _lang;

        public KillModule(IDbLanguage language)
        {
            _language = language;
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