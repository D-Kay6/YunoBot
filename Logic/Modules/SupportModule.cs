﻿namespace Logic.Modules
{
    using System.Threading.Tasks;
    using Discord.Commands;
    using IDal.Database;
    using Services;

    [Group("support")]
    public class SupportModule : ModuleBase<SocketCommandContext>
    {
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;

        public SupportModule(IDbLanguage language, LocalizationService localization)
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
        public async Task DefaultSupport()
        {
            await ReplyAsync(_localization.GetMessage("Support default"));
        }
    }
}