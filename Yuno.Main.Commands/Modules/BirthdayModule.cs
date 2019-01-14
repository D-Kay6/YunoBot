using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Yuno.Main.Extentions;

namespace Yuno.Main.Commands.Modules
{
    [Group("birthday")]
    public class BirthdayModule : ModuleBase<SocketCommandContext>
    {
        [Priority(-1)]
        [Command]
        public async Task DefaultBirthday([Remainder]string name)
        {
            await Context.Channel.SendMessageAsync($"Wait, who do you mean? I cannot find `{name}`.");
        }

        [Command]
        public async Task DefaultBirthday(SocketGuildUser user)
        {
            if (user == null)
            {
                await Context.Channel.SendMessageAsync($"Wait, who do you mean? I cannot find {Context.Message}.");
                return;
            }

            var name = user.Nickname ?? user.Username;
            var s = name.EndsWith("s") ? "" : "s";
            await Context.Channel.SendMessageAsync($@"Hooray! It's our little {name}'{s} birthday!

Happy Birthday to You :notes:
Happy Birthday to You
Happy Birthday Dear {name} :notes:
Happy Birthday to You.

From good friends and true, :notes:
From old friends and new,
May good luck go with you, :notes:
And happiness too.");
        }
    }
}
