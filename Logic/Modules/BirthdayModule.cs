using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;
using IDal.Interfaces.Database;
using Logic.Extensions;

namespace Logic.Modules
{
    [Group("birthday")]
    public class BirthdayModule : ModuleBase<SocketCommandContext>
    {
        private IDbLanguage _language;
        private Localization.Localization _lang;

        public BirthdayModule(IDbLanguage language)
        {
            _language = language;
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            Task.WaitAll(LoadLanguage());
            base.BeforeExecute(command);
        }

        private async Task LoadLanguage()
        {
            _lang = new Localization.Localization(await _language.GetLanguage(Context.Guild.Id));
        }

        [Priority(-1)]
        [Command]
        public async Task DefaultBirthday([Remainder] string name)
        {
            await ReplyAsync(_lang.GetMessage("Invalid user", name));
        }

        [Command]
        public async Task DefaultBirthday(SocketGuildUser user)
        {
            if (user == null)
            {
                await ReplyAsync(_lang.GetMessage("Invalid user", Context.Message));
                return;
            }

            var name = user.Nickname();
            await ReplyAsync(_lang.GetMessage("Birthday default", name.ToPossessive(), name));
        }
    }
}