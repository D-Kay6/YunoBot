using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Yuno.Main.Restart;

namespace Yuno.Main.Commands.Modules
{
    [RequireOwner]
    public class RestartModule : ModuleBase<SocketCommandContext>
    {
        [Command("restart")]
        public async Task RestartCommand()
        {
            await Context.Message.DeleteAsync();
            await ReplyAsync("Restarting...");
            RestartHandler.Restart();
        }

        [Command("shutdown")]
        public async Task ShutdownCommand()
        {
            await Context.Message.DeleteAsync();
            await ReplyAsync("Shutting down...");
            RestartHandler.Shutdown();
        }
    }
}
