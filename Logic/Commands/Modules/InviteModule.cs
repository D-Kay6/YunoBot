using Discord.Commands;
using IDal.Database;
using Logic.Extensions;
using Logic.Services;
using System.Threading.Tasks;

namespace Logic.Commands.Modules
{
    [Group("Invite")]
    public class InviteModule : ModuleBase<ShardedCommandContext>
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