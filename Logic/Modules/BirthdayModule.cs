﻿using Discord.Commands;
using Discord.WebSocket;
using Logic.Extentions;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Group("birthday")]
    public class BirthdayModule : ModuleBase<SocketCommandContext>
    {
        private Localization.Localization _lang;

        protected override void BeforeExecute(CommandInfo command)
        {
            _lang = new Localization.Localization(Context.Guild.Id);
            base.BeforeExecute(command);
        }

        [Priority(-1)]
        [Command]
        public async Task DefaultBirthday([Remainder] string name)
        {
            await Context.Channel.SendMessageAsync(_lang.GetMessage("Invalid user", name));
        }

        [Command]
        public async Task DefaultBirthday(SocketGuildUser user)
        {
            if (user == null)
            {
                await Context.Channel.SendMessageAsync(_lang.GetMessage("Invalid user", Context.Message));
                return;
            }

            var name = user.Nickname();
            await Context.Channel.SendMessageAsync(_lang.GetMessage("Birthday default", name.ToPossessive()));
        }
    }
}