using Discord;
using Discord.Commands;
using Discord.WebSocket;
using IDal.Interfaces.Database;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Group("Kick")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public class KickModule : ModuleBase<SocketCommandContext>
    {
        private IDbLanguage _language;
        private Localization.Localization _lang;

        public KickModule(IDbLanguage language)
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

        [Command]
        public async Task DefaultKick()
        {
            await ReplyAsync(_lang.GetMessage("Kick default"));
        }

        [Priority(-1)]
        [Command]
        public async Task KickUser([Remainder] string name)
        {
            await ReplyAsync(_lang.GetMessage("Invalid user", name));
        }

        [Command]
        public async Task KickUser(SocketGuildUser user, [Remainder] string message = null)
        {
            if (string.IsNullOrEmpty(message)) message = null;

            var builder = new EmbedBuilder();
            builder.AddField(_lang.GetMessage("Kick user"), user.Mention);

            var reason = message ?? _lang.GetMessage("Kick reason none");
            builder.AddField(_lang.GetMessage("Kick user"), reason);

            await ReplyAsync(null, false, builder.Build());
            await user.KickAsync(message);
        }
    }
}
