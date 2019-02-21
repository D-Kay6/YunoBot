using Discord;
using Discord.Commands;
using Logic.Extentions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Group("announce")]
    public class AnnounceModule : ModuleBase<SocketCommandContext>
    {
        [Priority(-1)]
        [Command]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DefaultAnnounce([Remainder] string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            foreach (var user in Context.Guild.Users)
            {
                await SendDM(user, message, Context.Guild.Name);
                await Task.Delay(150);
            }
        }

        [Command("global")]
        [RequireOwner]
        public async Task AnnounceGlobal([Remainder] string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            var usersDone = new HashSet<ulong>();
            foreach (var guild in Context.Client.Guilds)
            {
                if (usersDone.Contains(guild.OwnerId)) continue;
                if (guild.Id == 264445053596991498) continue;
                await SendDM(guild.Owner, message, "Update notice");
                usersDone.Add(guild.OwnerId);
                await Task.Delay(150);
            }
        }

        private async Task SendDM(IUser user, string message, string title = "")
        {
            var channel = await user.GetOrCreateDMChannelAsync();
            await channel.SendMessageAsync("", false, EmbedExtentions.CreateEmbed(title, message));
        }
    }
}