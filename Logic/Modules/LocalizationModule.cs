using Core.Enum;
using Discord;
using Discord.Commands;
using IDal.Database;
using Logic.Extensions;
using Logic.Services;
using System;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Group("language")]
    [Alias("lang")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class LocalizationModule : ModuleBase<ShardedCommandContext>
    {
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;

        public LocalizationModule(IDbLanguage language, LocalizationService localization)
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
        public async Task DefaultLanguage()
        {
            var language = await _language.GetLanguage(Context.Guild.Id);
            await ReplyAsync(_localization.GetMessage("Language default", language, Context.Guild.Name,
                string.Join(", ", Enum.GetNames(typeof(Language)))));
        }

        [Command("set")]
        public async Task LanguageSet(string value)
        {
            if (!Enum.TryParse(value.FirstCharToUpper(), out Language language))
            {
                await ReplyAsync(_localization.GetMessage("Language unsupported", value));
                return;
            }

            await _language.SetLanguage(Context.Guild.Id, language);
            await Prepare();
            await ReplyAsync(_localization.GetMessage("Language set", language, Context.Guild.Name));
        }
    }
}