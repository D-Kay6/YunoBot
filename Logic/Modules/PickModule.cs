using Discord;
using Discord.Commands;
using IDal.Database;
using Logic.Extensions;
using Logic.Services;
using System;
using System.Threading.Tasks;

namespace Logic.Modules
{
    public class PickModule : ModuleBase<SocketCommandContext>
    {
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;

        public PickModule(IDbLanguage language, LocalizationService localization)
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

        [Command("pick")]
        public async Task PickCommand([Remainder] string message)
        {
            var options = message.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
            var embed = EmbedExtensions.CreateEmbed(_localization.GetMessage("Pick default", Context.User.Username),
                options.GetRandomItem(),
                new Color(255, 255, 0));

            await ReplyAsync("", false, embed);
        }
    }
}