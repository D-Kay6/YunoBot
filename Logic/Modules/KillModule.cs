using Discord.Commands;
using Discord.WebSocket;
using Logic.Extentions;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Group("kill")]
    public class KillModule : ModuleBase<SocketCommandContext>
    {
        [Priority(-1)]
        [Command]
        public async Task DefaultCommand([Remainder] string name)
        {
            await Context.Channel.SendMessageAsync($"Wait, who do you mean? I cannot find `{name}`.");
        }

        [Command]
        public async Task DefaultCommand(SocketGuildUser user)
        {
            if (user == null) return;
            switch (user.Id)
            {
                case 255453041531158538:
                    await ReplyAsync(
                        $"How dare you to order me to kill {user.Nickname()}! \nI should kill YOU for even thinking of something like that!");
                    break;
                case 286972781273546762:
                    await ReplyAsync("Why would I kill myself?");
                    break;
                default:
                    await Context.Channel.SendFileAsync(ImageExtentions.GetImagePath("GasaiYuno.gif"),
                        $"Come here {user.Mention}. We're gonna have some fun...");
                    break;
            }
        }
    }
}