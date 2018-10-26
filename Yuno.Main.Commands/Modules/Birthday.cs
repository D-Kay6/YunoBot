using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace Yuno.Main.Commands.Modules
{
    public class Birthday : ModuleBase<SocketCommandContext>
    {
        [Command("birthday")]
        public async Task Command([Remainder] string message)
        {
            var users = new HashSet<SocketGuildUser>();
            Context.Channel.GetUsersAsync().ForEach(u => users.UnionWith(u.Cast<SocketGuildUser>()));
            var user = users.FirstOrDefault(u => u.Username.Equals(message, StringComparison.CurrentCultureIgnoreCase)) ??
                       users.Where(u => !string.IsNullOrEmpty(u.Nickname)).FirstOrDefault(u => u.Nickname.Equals(message, StringComparison.CurrentCultureIgnoreCase)) ??
                       users.FirstOrDefault(u => u.Username.Contains(message));
            if (user == null)
            {
                await Context.Channel.SendMessageAsync($"Wait, who do you mean? I cannot find '{message}'.");
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
