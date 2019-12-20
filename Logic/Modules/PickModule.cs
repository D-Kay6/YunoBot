﻿using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using IDal.Interfaces.Database;
using Logic.Extensions;

namespace Logic.Modules
{
    public class PickModule : ModuleBase<SocketCommandContext>
    {
        private IDbLanguage _language;
        private Localization.Localization _lang;

        public PickModule(IDbLanguage language)
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

        [Command("pick")]
        public async Task PickCommand([Remainder] string message)
        {
            var options = message.Split(new[] {'|'}, StringSplitOptions.RemoveEmptyEntries);
            var embed = EmbedExtensions.CreateEmbed(_lang.GetMessage("Pick default", Context.User.Username),
                options.GetRandomItem(),
                new Color(255, 255, 0));

            await ReplyAsync("", false, embed);
        }
    }
}