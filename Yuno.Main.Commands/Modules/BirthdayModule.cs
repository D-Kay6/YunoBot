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
    public class BirthdayModule : ModuleBase<SocketCommandContext>
    {
        [Command("birthday")]
        public async Task BirthdayCommand([Remainder] string message)
        {
            var user = Context.Channel.TryGetUser(message).Result;
            if (user == null) return;

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
