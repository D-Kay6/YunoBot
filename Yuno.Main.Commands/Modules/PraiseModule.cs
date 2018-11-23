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
        [Priority(-1)]
        [Command]
        public async Task DefaultPraise([Remainder]string message)
        {
            var user = Context.Channel.TryGetUser(message).Result;
            await PraiseUser(user, message);
        }

        private async Task PraiseUser(SocketGuildUser user, string message = null)
        {
            if (user == null) return;
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
                    await ReplyAsync($"Good job, {user.Mention}. You deserve a :cookie:.", false);
                    return;
            }
        }
        
        [Command("everyone")]
        public async Task PraiseEveryone()
        {
            await ReplyAsync($"Yay! You're all such lovely people :heart:.", false);
            await ReplyAsync($"But don't be getting any ideas now. \nNo one could even get close to my wonderful D-Kay.", false);
        }
        
        [Command("someone")]
        public async Task PraiseSomeone()
        {
            var user = Context.Channel.GetRandomUser();
            await PraiseUser(user);
        }
        
        [Command("child")]
        public async Task PraiseChild()
        {
            await ReplyAsync("I am not gonna do that. Are you sure you did not mean /kill child?");
        }
        
        [Command("satan")]
        public async Task PraiseDKay()
        {
            await ReplyAsync("All hail the bringer of pain and destruction *(and my future husband)*, D-Kay :heart:.", false);
        }
        
        [Command("god")]
        public async Task PraiseDemanicus()
        {
            await ReplyAsync("Wait, are you sure you mean him? Ok then. \nAll hail our lord and savior, Demanicus.", false);
        }
        
        [Command("wizard")]
        public async Task PraiseJim()
        {
            await ReplyAsync("I cannot imagine he deserves it but, \nAll hail our powerful and destructive wizard, jim.", false);
        }
    }
}
