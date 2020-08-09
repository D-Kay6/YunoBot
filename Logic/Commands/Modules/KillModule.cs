using Discord.Commands;
using Discord.WebSocket;
using IDal.Database;
using Logic.Extensions;
using Logic.Services;
using System.Threading.Tasks;

namespace Logic.Commands.Modules
{
    [Group("kill")]
    public class KillModule : ModuleBase<ShardedCommandContext>
    {
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;

        public KillModule(IDbLanguage language, LocalizationService localization)
        {
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

        [Priority(-1)]
        [Command]
        public async Task DefaultCommand([Remainder] string name)
        {
            await ReplyAsync(_localization.GetMessage("Invalid user", name));
        }

        [Command]
        public async Task DefaultCommand(SocketGuildUser user)
        {
            if (user == null) return;
            switch (user.Id)
            {
                case 255453041531158538:
                    await ReplyAsync(_localization.GetMessage("Kill creator", user.Nickname()));
                    break;
                case 286972781273546762:
                    await ReplyAsync(_localization.GetMessage("Kill self"));
                    break;
                default:
                    await Context.Channel.SendFileAsync(ImageExtensions.GetImagePath("GasaiYuno.gif"),
                        _localization.GetMessage("Kill default", user.Mention));
                    break;
            }
        }
    }
}