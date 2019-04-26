using Discord;
using Discord.Commands;
using Logic.Extentions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Net;
using Logic.Handlers;

namespace Logic.Modules
{
    [Group("support")]
    public class SupportModule : ModuleBase<SocketCommandContext>
    {
        [Command]
        public async Task DefaultSupport()
        {
            await ReplyAsync($@"I'm sorry to hear you have problems with me. Come join my support server 'https://discord.gg/YFqUMDT'. Maybe someone there can help you.");
        }
    }
}