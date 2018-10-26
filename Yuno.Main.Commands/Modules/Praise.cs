using System;
using System.Collections.Generic;
using System.Linq;
using Discord.Commands;
using System.Threading.Tasks;
using Discord.WebSocket;
using Yuno.Main.Extentions;

namespace Yuno.Main.Commands.Modules
{
    public class Praise : ModuleBase<SocketCommandContext>
    {
        private Random _random = new Random();

        [Command("praise")]
        public async Task Command([Remainder]string message)
        {
            var users = new HashSet<SocketGuildUser>();
            Context.Channel.GetUsersAsync().ForEach(u => users.UnionWith(u.Cast<SocketGuildUser>()));
            SocketGuildUser user;
            switch (message.ToLower())
            {
                case "d-kay":
                case "satan":
                    await Context.Channel.SendMessageAsync("All hail the bringer of pain and destruction *(and my future husband)*, D-Kay :heart:.", false);
                    return;
                case "demanicus":
                case "god":
                    await Context.Channel.SendMessageAsync("Wait, are you sure you mean him? Ok then.", false);
                    await Context.Channel.SendMessageAsync("All hail our lord and savior, Demanicus.", false);
                    return;
                case "hygzni":
                case "jim":
                    await Context.Channel.SendMessageAsync("I cannot imagine he deserves it but sure.", false);
                    await Context.Channel.SendMessageAsync($"All hail our powerful and destructive wizard, {message}.", false);
                    return;
                case "everyone":
                    await Context.Channel.SendMessageAsync($"Yay! You're all such lovely people :heart:.", false);
                    await Context.Channel.SendMessageAsync($"But don't you dare to lay a finger on D-Kay unless you want me to come pay you a visit :knife:.", false);
                    return;
                case "someone":
                    users.RemoveWhere(u => u.IsBot);
                    user = users.ElementAt(_random.Next(users.Count));
                    break;
                default:
                    user = users.FirstOrDefault(u => u.Username.Equals(message, StringComparison.CurrentCultureIgnoreCase)) ??
                           users.Where(u => !string.IsNullOrEmpty(u.Nickname)).FirstOrDefault(u => u.Nickname.Equals(message, StringComparison.CurrentCultureIgnoreCase)) ??
                           users.FirstOrDefault(u => u.Username.Contains(message));
                    if (user == null)
                    {
                        await Context.Channel.SendMessageAsync($"Wait, who do you mean? I cannot find '{message}'.");
                        return;
                    }
                    break;
            }
            await PraiseUser(user);
        }

        private async Task PraiseUser(SocketGuildUser user)
        {
            if (user == null) return;
            await Context.Channel.SendMessageAsync($"Good job, {user.Mention}. You deserve a :cookie:.", false);
        }
    }
}
