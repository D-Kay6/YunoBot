using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;

namespace Yuno.Main.Commands.Modules
{
    public class AutoChannel : ModuleBase<SocketCommandContext>
    {
        [Alias("ac")]
        [Command("autochannel")]
        public async Task Command([Remainder] string message)
        {
            IEnumerable<string> args = message.Split(' ');
            if (!args.Any())
            {
                await Context.Channel.SendMessageAsync($@"/ac seticon (icon)");
                return;
            }
            switch (args.First().ToLower())
            {
                case "seticon":
                    break;
                default:
                    await Context.Channel.SendMessageAsync("Incorrect command usage.");
                    break;
            }
        }
    }
}
