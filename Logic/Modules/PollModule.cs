using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace Logic.Modules
{
    [Group("poll")]
    public class PollModule : ModuleBase<SocketCommandContext>
    {
        [Command]
        public async Task DefaultPoll([Remainder] string message)
        {
            return;
            await Context.Message.DeleteAsync();
            var embed = new EmbedBuilder();
            embed.WithDescription("A poll has started!");
            await ReplyAsync();
        }
    }
}
