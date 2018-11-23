using System;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yuno.Main.Extentions
{
    public static class UserExtentions
    {
        private static Random _random = new Random();

        public static List<SocketGuildUser> GetUsers(this ISocketMessageChannel channel)
        {
            var users = new List<SocketGuildUser>();
            channel.GetUsersAsync().ForEachAsync(u => users.AddRange(u.Cast<SocketGuildUser>()));
            return users;
        }

        public static SocketGuildUser GetRandomUser(this ISocketMessageChannel channel)
        {
            var users = channel.GetUsers();
            users.RemoveAll(u => u.IsBot);
            return users[_random.Next(users.Count)];
        }

        public static SocketGuildUser GetUser(this ISocketMessageChannel channel, string name)
        {
            return channel.GetUsers().GetUser(name);
        }

        public static SocketGuildUser GetUser(this IEnumerable<SocketGuildUser> list, string name)
        {
            return list.FirstOrDefault(u => u.Username.Equals(name, StringComparison.CurrentCultureIgnoreCase)) ??
                   list.Where(u => !string.IsNullOrEmpty(u.Nickname)).FirstOrDefault(u => u.Nickname.Equals(name, StringComparison.CurrentCultureIgnoreCase)) ??
                   list.FirstOrDefault(u => u.Username.ToLower().Contains(name.ToLower())) ??
                   list.Where(u => !string.IsNullOrEmpty(u.Nickname)).FirstOrDefault(u => u.Nickname.ToLower().Contains(name.ToLower()));
        }

        public static async Task<SocketGuildUser> TryGetUser(this ISocketMessageChannel channel, string name)
        {
            var users = channel.GetUsers();
            var user = users.FirstOrDefault(u => u.Username.Equals(name, StringComparison.CurrentCultureIgnoreCase)) ??
                   users.Where(u => !string.IsNullOrEmpty(u.Nickname)).FirstOrDefault(u => u.Nickname.Equals(name, StringComparison.CurrentCultureIgnoreCase));
            if (user != null) return user;
            var cList = users.Where(u => u.Username.ToLower().Contains(name.ToLower()));
            switch (cList.Count())
            {
                case 1:
                    return cList.First();
                case 0:
                    break;
                default:
                    await channel.SendMessageAsync($"I found {cList.Count()} users matching '{name}'. Please be more specific.");
                    return null;
            }
            var nList = users.Where(u => !string.IsNullOrEmpty(u.Nickname)).Where(u => u.Nickname.ToLower().Contains(name.ToLower()));
            switch (nList.Count())
            {
                case 1:
                    return nList.First();
                case 0:
                    await channel.SendMessageAsync($"Wait, who do you mean? I cannot find {name}.");
                    return null;
                default:
                    await channel.SendMessageAsync($"I found {cList.Count()} users matching '{name}'. Please be more specific.");
                    return null;
            }
        }
    }
}
