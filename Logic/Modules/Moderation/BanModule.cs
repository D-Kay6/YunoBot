namespace Logic.Modules.Moderation
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using Extensions;
    using IDal.Database;
    using Services;

    [Group("Ban")]
    [RequireUserPermission(GuildPermission.BanMembers)]
    public class BanModule : ModuleBase<SocketCommandContext>
    {
        private readonly IDbBan _ban;
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;
        private readonly IDbUser _user;

        public BanModule(IDbBan ban, IDbUser user, IDbLanguage language, LocalizationService localization)
        {
            _ban = ban;
            _user = user;
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
        public async Task DefaultBan()
        {
            await ReplyAsync(_localization.GetMessage("Ban default"));
        }

        [Priority(-1)]
        [Command]
        public async Task BanUser([Remainder] string name)
        {
            await ReplyAsync(_localization.GetMessage("Invalid user", name));
        }

        [Command]
        public async Task BanUser(SocketGuildUser user)
        {
            var builder = new EmbedBuilder();
            builder.AddField(_localization.GetMessage("Ban user"), user.Mention);
            var time = _localization.GetMessage("Ban duration forever");
            var reason = _localization.GetMessage("Ban reason none");

            builder.AddField(_localization.GetMessage("Ban duration"), time);
            builder.AddField(_localization.GetMessage("Ban reason"), reason);
            await ReplyAsync(null, false, builder.Build());
            await user.BanAsync();
        }

        [Command]
        public async Task BanUser(SocketGuildUser user, [Remainder] string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                await BanUser(user);
                return;
            }

            var builder = new EmbedBuilder();
            builder.AddField(_localization.GetMessage("Ban user"), user.Mention);
            var time = _localization.GetMessage("Ban duration forever");
            var reason = _localization.GetMessage("Ban reason none");

            var parts = message.Split(' ');
            var duration = parts[0].GetDuration();
            var endDate = duration > TimeSpan.Zero ? DateTime.Now + duration : (DateTime?) null;
            if (endDate != null) time = endDate.Value.ToString();

            message = string.Join(' ', parts.Skip(1));
            if (!string.IsNullOrEmpty(message)) reason = message;

            builder.AddField(_localization.GetMessage("Ban duration"), time);
            builder.AddField(_localization.GetMessage("Ban reason"), reason);

            await ReplyAsync(null, false, builder.Build());
            await _user.UpdateUser(user.Id, user.Username);
            await _ban.AddBan(user.Id, user.Guild.Id, endDate, reason);
            await user.BanAsync(0, string.IsNullOrEmpty(message) ? null : reason);
        }
    }
}