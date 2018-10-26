using System;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Linq;

namespace Yuno.Main.Extentions
{
    public static class UserExtentions
    {
        public static List<SocketGuildUser> GetUsers(this ISocketMessageChannel channel)
        {
            var users = new List<SocketGuildUser>();
            channel.GetUsersAsync().ForEachAsync(u => users.AddRange(u.Cast<SocketGuildUser>()));
            return users;
        }

        public static SocketGuildUser GetUser(this ISocketMessageChannel channel, string name)
        {
            return channel.GetUsers().GetUser(name);
        }

        public static SocketGuildUser GetUser(this IEnumerable<SocketGuildUser> list, string name)
        {
            return list.FirstOrDefault(u => u.Username.Equals(name, StringComparison.CurrentCultureIgnoreCase)) ??
                   list.Where(u => !string.IsNullOrEmpty(u.Nickname)).FirstOrDefault(u => u.Nickname.Equals(name, StringComparison.CurrentCultureIgnoreCase)) ??
                   list.FirstOrDefault(u => u.Username.Contains(name));
        }
    }
}
