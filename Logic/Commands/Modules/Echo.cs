using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Logic.Extentions;

namespace Logic.Commands.Modules
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
