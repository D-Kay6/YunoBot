using Discord.Commands;
using Logic.Handlers;
using System.Threading.Tasks;

namespace Logic.Modules
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