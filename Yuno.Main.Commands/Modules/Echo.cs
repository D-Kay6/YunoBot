using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Yuno.Main.Extentions;

namespace Yuno.Main.Commands.Modules
{
    public class Echo : ModuleBase<SocketCommandContext>
    {
        [Command("echo")]
        public async Task Command([Remainder] string message)
        {
            var embed = EmbedExtention.CreateEmbed($"Message by {Context.User.Username}", message, Color.Green);
            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
