using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Yuno.Main.Extentions;

namespace Yuno.Main.Commands.Modules
{
    public class KillModule : ModuleBase<SocketCommandContext>
    {
        [Command("kill")]
        public async Task Command([Remainder] string message)
        {
            var user = Context.Channel.GetUser(message);
            if (user == null)
            {
                await ReplyAsync($"Wait, who do you mean? I cannot find '{message}'.");
                return;
            }

            if (user.Id == 255453041531158538)
            {
                await ReplyAsync($"How dare you to order me to kill {user.Nickname ?? user.Username}! \n I should kill YOU for even thinking of something like that!");
                return;
            }

            var embed = EmbedExtention.CreateEmbed("", $"Come here {user.Mention}. We're gonna have some fun...",
                Color.DarkRed, "https://pa1.narvii.com/6008/51cb30b589766f4e8f5b0ff09da77b7ee20453d7_hq.gif");
            await ReplyAsync("", false, embed);
        }
    }
}
