using DalFactory;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using IDal.Interfaces.Database;

namespace Logic.Modules
{
    [Group("prefix")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class PrefixModule : ModuleBase<SocketCommandContext>
    {
        private ICommand _command;
        private Localization.Localization _lang;

        public PrefixModule(ICommand command)
        {
            _command = command;
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            _lang = new Localization.Localization(Context.Guild.Id);
            base.BeforeExecute(command);
        }

        [Command]
        public async Task DefaultPrefix()
        {
            await ReplyAsync(_lang.GetMessage("Prefix default", _command.GetPrefix(Context.Guild.Id), Context.Guild.Name));
        }

        [Command("set")]
        public async Task PrefixSet([Remainder] string message)
        {
            _command.SetPrefix(Context.Guild.Id, message);
            await ReplyAsync(_lang.GetMessage("Prefix set", message, Context.Guild.Name));
        }
    }
}