using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Yuno.Main.Extentions;

namespace Yuno.Main.Commands.Modules
{
    [Group("kill")]
    public class KillModule : ModuleBase<SocketCommandContext>
    {
        [Priority(-1)]
        [Command]
        public async Task DefaultCommand([Remainder] string message)
        {
            var user = Context.Channel.TryGetUser(message).Result;
            if (user == null) return;

            switch (user.Id)
            {
                case 255453041531158538:
                    await ReplyAsync($"How dare you to order me to kill {user.Nickname ?? user.Username}! \n I should kill YOU for even thinking of something like that!");
                    break;
                case 240938068964671500:
                    await KillChild();
                    break;
                default:
                    await Context.Channel.SendFileAsync(ImageExtention.GetImagePath("GasaiYuno.gif"),
                        $"Come here {user.Mention}. We're gonna have some fun...");
                    break;
            }
        }
        
        [Command("child")]
        public async Task KillChild()
        {
            await Context.Channel.SendFileAsync(ImageExtention.GetImagePath("GasaiYunoCrazy.png"),
                @"No no no, killing him would be way too kind. How about some torture? Sounds good, doesn't it?
Maybe the bronze bull? No, still kills him....
I know! The thumbscrew and it's many variations! I'd gladly break all of his bones piece by piece. It's not like he's gonna need his arms or legs anyway.");
        }
    }
}