using Discord;
using Discord.Commands;
using Discord.Net;
using Logic.Extensions;
using Logic.Handlers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Group("announce")]
    public class AnnounceModule : ModuleBase<SocketCommandContext>
    {
        private Localization.Localization _lang;

        protected override void BeforeExecute(CommandInfo command)
        {
            _lang = new Localization.Localization(Context.Guild.Id);
            base.BeforeExecute(command);
        }

        [Priority(-1)]
        [Command]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DefaultAnnounce([Remainder] string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                await ReplyAsync(_lang.GetMessage("Invalid message"));
                return;
            }

            SendAnnouncement(Context.Guild.Users, message, Context.Guild.Name);
        }

        [Command("global")]
        [RequireOwner]
        public async Task AnnounceGlobal([Remainder] string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                await ReplyAsync(_lang.GetMessage("Invalid message"));
                return;
            }
            
            var users = new List<IUser>();
            foreach (var guild in Context.Client.Guilds)
            {
                if (guild.Id == 264445053596991498) continue;
                if (users.Any(u => u.Id.Equals(guild.OwnerId))) continue;
                users.Add(guild.Owner);
            }
            SendAnnouncement(users, message, "Update notice");
        }

        private async Task SendAnnouncement(IReadOnlyCollection<IUser> users, string message, string title = "")
        {
            users.Foreach(async u => await SendDM(u, message, title));
        }

        private async Task SendDM(IUser user, string message, string title = "")
        {
            try
            {
                var channel = await user.GetOrCreateDMChannelAsync();
                await channel.SendMessageAsync("", false, EmbedExtensions.CreateEmbed(title, message));
                LogsHandler.Instance.Log("Announcement", $"{user.Username}({user.Id}) - {message}");
            }
            catch (HttpException)
            {
                LogsHandler.Instance.Log("Announcement", $"Could not send announcement to {user.Username}({user.Id}).");
            }
        }
    }
}