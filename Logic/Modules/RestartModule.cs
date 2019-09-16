using Discord.Commands;
using Logic.Handlers;
using System.Threading.Tasks;
using Logic.Services;

namespace Logic.Modules
{
    [RequireOwner]
    public class RestartModule : ModuleBase<SocketCommandContext>
    {
        private Localization.Localization _lang;

        public RestartService Service { get; set; }

        protected override void BeforeExecute(CommandInfo command)
        {
            _lang = new Localization.Localization(Context.Guild.Id);
            base.BeforeExecute(command);
        }

        [Command("restart")]
        public async Task RestartCommand()
        {
            await Context.Message.DeleteAsync();
            await ReplyAsync(_lang.GetMessage("Restart default"));
            Service.Restart();
        }

        [Command("shutdown")]
        public async Task ShutdownCommand()
        {
            await Context.Message.DeleteAsync();
            await ReplyAsync(_lang.GetMessage("Shutdown default"));
            Service.Shutdown();
        }
    }
}