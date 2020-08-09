using Discord;
using Discord.Commands;
using Discord.Net;
using IDal.Database;
using Logic.Extensions;
using Logic.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Commands.Modules
{
    [Group("announce")]
    public class AnnounceModule : ModuleBase<ShardedCommandContext>
    {
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;
        private readonly LogsService _logs;

        public AnnounceModule(IDbLanguage language, LocalizationService localization, LogsService logs)
        {
            _language = language;
            _localization = localization;
            _logs = logs;
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
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DefaultAnnounce([Remainder] string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                await ReplyAsync(_localization.GetMessage("Invalid message"));
                return;
            }

            await SendAnnouncement(Context.Guild.Users, message, Context.Guild.Name);
        }

        [Command("global")]
        [RequireOwner]
        public async Task AnnounceGlobal([Remainder] string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                await ReplyAsync(_localization.GetMessage("Invalid message"));
                return;
            }

            var users = new List<IUser>();
            foreach (var guild in Context.Client.Guilds)
            {
                if (guild.Id == 264445053596991498) continue;
                if (users.Any(u => u.Id.Equals(guild.OwnerId))) continue;
                users.Add(guild.Owner);
            }

            await SendAnnouncement(users, message, "Update notice");
        }

        private async Task SendAnnouncement(IReadOnlyCollection<IUser> users, string message, string title = "")
        {
            foreach (var user in users)
            {
                await SendDM(user, message, title);
                await Task.Delay(1000);
            }
        }

        private async Task SendDM(IUser user, string message, string title = "")
        {
            try
            {
                var channel = await user.GetOrCreateDMChannelAsync();
                await channel.SendMessageAsync("", false, EmbedExtensions.CreateEmbed(title, message));
                await _logs.Write("Announcement", $"{user.Username}({user.Id}) - {message}");
            }
            catch (HttpException)
            {
                await _logs.Write("Announcement", $"Could not send announcement to {user.Username}({user.Id}).");
            }
        }
    }
}