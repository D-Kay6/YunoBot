using System;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using DalFactory;
using IDal.Interfaces.Database;
using IDal.Structs.Localization;
using Logic.Extensions;

namespace Logic.Modules
{
    [Group("language")]
    [Alias("lang")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class LocalizationModule : ModuleBase<SocketCommandContext>
    {
        private IServerSettings _settings;
        private Localization.Localization _lang;

        protected override void BeforeExecute(CommandInfo command)
        {
            _settings = DatabaseFactory.GenerateServerSettings();
            _lang = new Localization.Localization(Context.Guild.Id);
            base.BeforeExecute(command);
        }

        [Command]
        public async Task DefaultLanguage()
        {
            var language = _settings.GetLanguage(Context.Guild.Id);
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

            _settings.SetLanguage(Context.Guild.Id, language);
            _lang = new Localization.Localization(Context.Guild.Id);
            await ReplyAsync(_lang.GetMessage("Language set", language, Context.Guild.Name));
        }
    }
}
