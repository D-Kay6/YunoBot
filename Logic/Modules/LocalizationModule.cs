using Discord;
using Discord.Commands;
using Entity;
using IDal.Interfaces.Database;
using Logic.Extensions;
using System;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Group("language")]
    [Alias("lang")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class LocalizationModule : ModuleBase<SocketCommandContext>
    {
        private IDbLanguage _language;
        private Localization.Localization _lang;

        public LocalizationModule(IDbLanguage language)
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

        [Command]
        public async Task DefaultLanguage()
        {
            var language = await _language.GetLanguage(Context.Guild.Id);
            await ReplyAsync(_lang.GetMessage("Language default", language, Context.Guild.Name, string.Join(", ", Enum.GetNames(typeof(Language)))));
        }

        [Command("set")]
        public async Task LanguageSet(string value)
        {
            if (!Enum.TryParse(value.FirstCharToUpper(), out Language language))
            {
                await ReplyAsync(_lang.GetMessage("Language unsupported", value));
                return;
            }

            await _language.SetLanguage(Context.Guild.Id, language);
            await LoadLanguage();
            await ReplyAsync(_lang.GetMessage("Language set", language, Context.Guild.Name));
        }
    }
}
