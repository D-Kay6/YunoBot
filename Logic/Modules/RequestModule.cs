using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Logic.Handlers;

namespace Logic.Modules
{
    [Group("request")]
    public class RequestModule : ModuleBase<SocketCommandContext>
    {
        [Command]
        public async Task DefaultRequest()
        {
            await ReplyAsync("Send some information to me about your idea for a new feature or if you think something needs to be changed.");
        }

        [Command("feature")]
        public async Task RequestFeature([Remainder] string message)
        {
            LogsHandler.Instance.Log("Features", message);
            await ReplyAsync("Thanks for your submission.");
        }

        [Command("change")]
        public async Task RequestChange([Remainder] string message)
        {
            LogsHandler.Instance.Log("Changes", message);
            await ReplyAsync("Thanks for your submission.");
        }
    }
}
