namespace Logic.Modules
{
    using System.Threading.Tasks;
    using Discord.Commands;
    using Extensions;
    using IDal.Database;
    using Services;

    [Group("Invite")]
    public class InviteModule : ModuleBase<SocketCommandContext>
    {
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;

        public InviteModule(IDbLanguage language, LocalizationService localization)
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

        [Command]
        public async Task DefaultInvite()
        {
            await Context.User.SendDM(_localization.GetMessage("Invite default"));
        }
    }
}