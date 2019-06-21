using DalFactory;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Group("prefix")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class PrefixModule : ModuleBase<SocketCommandContext>
    {
        private Localization.Localization _lang;

        protected override void BeforeExecute(CommandInfo command)
        {
            _lang = new Localization.Localization(Context.Guild.Id);
            base.BeforeExecute(command);
        }

        [Command]
        public async Task DefaultPrefix()
        {
            var settings = DatabaseFactory.GenerateServerSettings();
            await ReplyAsync(_lang.GetMessage("Prefix default", settings.GetCommandPrefix(Context.Guild.Id), Context.Guild.Name));
        }

        [Command("set")]
        public async Task PrefixSet([Remainder] string message)
        {
            var settings = DatabaseFactory.GenerateServerSettings();
            settings.SetCommandPrefix(Context.Guild.Id, message);
            await ReplyAsync(_lang.GetMessage("Prefix set", message, Context.Guild.Name));
        }
    }
}