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
        private ILanguage _language;
        private Localization.Localization _lang;

        public LocalizationModule(ILanguage language)
        {
            _language = language;
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            _lang = new Localization.Localization(Context.Guild.Id);
            base.BeforeExecute(command);
        }

        [Command]
        public async Task DefaultLanguage()
        {
            var language = _language.GetLanguage(Context.Guild.Id);
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

            _language.SetLanguage(Context.Guild.Id, language);
            _lang = new Localization.Localization(Context.Guild.Id);
            await ReplyAsync(_lang.GetMessage("Language set", language, Context.Guild.Name));
        }
    }
}
