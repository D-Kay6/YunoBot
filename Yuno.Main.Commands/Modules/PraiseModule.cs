using System;
using System.Collections.Generic;
using System.Linq;
using Discord.Commands;
using System.Threading.Tasks;
using Discord.WebSocket;
using Yuno.Main.Extentions;

namespace Yuno.Main.Commands.Modules
{
    [Group("praise")]
    public class PraiseModule : ModuleBase<SocketCommandContext>
    {
        private Random _random = new Random();

        [Command("praise")]
        public async Task Command([Remainder]string message)
        {
            var users = Context.Channel.GetUsers();
            SocketGuildUser user;
            switch (message.ToLower())
            {
                case "satan":
                    await PraiseDKay();
                    return;
                case "god":
                    await PraiseDemanicus();
                    return;
                case "wizard":
                    await PraiseJim();
                    return;
                case "everyone":
                    await ReplyAsync($"Yay! You're all such lovely people :heart:.", false);
                    await ReplyAsync($"But don't be getting any ideas now. \nNo one could even get close to my wonderful D-Kay.", false);
                    return;
                case "someone":
                    users.RemoveAll(u => u.IsBot);
                    user = users.ElementAt(_random.Next(users.Count));
                    break;
                default:
                    user = users.GetUser(message);
                    if (user == null)
                    {
                        await ReplyAsync($"Wait, who do you mean? I cannot find '{message}'.");
                        return;
                    }
                    break;
            }

            switch (user.Id)
            {
                case 255453041531158538:
                    await PraiseDKay();
                    return;
                case 235587420777611265:
                    await PraiseDemanicus();
                    return;
                case 283233504156975105:
                    await PraiseJim();
                    return;
                default:
                    await PraiseUser(user);
                    return;
            }
        }

        private async Task PraiseUser(SocketGuildUser user)
        {
            if (user == null) return;
            await ReplyAsync($"Good job, {user.Mention}. You deserve a :cookie:.", false);
        }

        private async Task PraiseDKay()
        {
            await ReplyAsync("All hail the bringer of pain and destruction *(and my future husband)*, D-Kay :heart:.", false);
        }

        private async Task PraiseDemanicus()
        {
            await ReplyAsync("Wait, are you sure you mean him? Ok then. \nAll hail our lord and savior, Demanicus.", false);
        }

        private async Task PraiseJim()
        {
            await ReplyAsync("I cannot imagine he deserves it but, \nAll hail our powerful and destructive wizard, {message}.", false);
        }
    }
}
