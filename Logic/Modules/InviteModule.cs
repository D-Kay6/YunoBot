using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace Logic.Modules
{
    [Group("Invite")]
    public class InviteModule : ModuleBase<SocketCommandContext>
    {
        [Command]
        public async Task DefaultInvite()
        {
            var channel = await Context.User.GetOrCreateDMChannelAsync();
            await channel.SendMessageAsync(@"You can invite me to your server with this link: https://discordapp.com/oauth2/authorize?client_id=286972781273546762&scope=bot&permissions=8.
For more info on me, go take a look at https://discordbots.org/bot/286972781273546762, or use /help in any of the discord servers I'm in.");
        }
    }
}
