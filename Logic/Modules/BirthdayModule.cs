namespace Logic.Modules
{
    using System.Threading.Tasks;
    using Discord.Commands;
    using Discord.WebSocket;
    using Extensions;
    using IDal.Database;
    using Services;

    [Group("birthday")]
    public class BirthdayModule : ModuleBase<SocketCommandContext>
    {
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;

        public BirthdayModule(IDbLanguage language, LocalizationService localization)
        {
            _language = language;
            _localization = localization;
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            Task.WaitAll(Prepare());
            base.BeforeExecute(command);
        }

        private async Task Prepare()
        {
            await _localization.Load(await _language.GetLanguage(Context.Guild.Id));
        }

        [Priority(-1)]
        [Command]
        public async Task DefaultBirthday([Remainder] string name)
        {
            await ReplyAsync(_localization.GetMessage("Invalid user", name));
        }

        [Command]
        public async Task DefaultBirthday(SocketGuildUser user)
        {
            if (user == null)
            {
                await ReplyAsync(_localization.GetMessage("Invalid user", Context.Message));
                return;
            }

            var name = user.Nickname();
            await ReplyAsync(_localization.GetMessage("Birthday default", name.ToPossessive(), name));
        }
    }
}