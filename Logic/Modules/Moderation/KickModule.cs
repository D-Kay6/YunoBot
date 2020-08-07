using Discord;
using Discord.Commands;
using Discord.WebSocket;
using IDal.Database;
using Logic.Services;
using System.Threading.Tasks;

namespace Logic.Modules.Moderation
{
    [Group("Kick")]
    [RequireUserPermission(GuildPermission.KickMembers)]
    public class KickModule : ModuleBase<ShardedCommandContext>
    {
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;

        public KickModule(IDbLanguage language, LocalizationService localization)
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

        [Command]
        public async Task DefaultKick()
        {
            await ReplyAsync(_localization.GetMessage("Kick default"));
        }

        [Priority(-1)]
        [Command]
        public async Task KickUser([Remainder] string name)
        {
            await ReplyAsync(_localization.GetMessage("Invalid user", name));
        }

        [Command]
        public async Task KickUser(SocketGuildUser user, [Remainder] string message = null)
        {
            if (string.IsNullOrEmpty(message)) message = null;

            var builder = new EmbedBuilder();
            builder.AddField(_localization.GetMessage("Kick user"), user.Mention);

            var reason = message ?? _localization.GetMessage("Kick reason none");
            builder.AddField(_localization.GetMessage("Kick user"), reason);

            await ReplyAsync(null, false, builder.Build());
            await user.KickAsync(message);
        }
    }
}