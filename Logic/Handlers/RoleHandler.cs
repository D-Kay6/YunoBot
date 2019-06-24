using DalFactory;
using Discord;
using Discord.WebSocket;
using IDal.Interfaces.Database;
using System;
using System.Linq;
using System.Threading.Tasks;
using Logic.Extensions;

namespace Logic.Handlers
{
    public class RoleHandler
    {
        private DiscordSocketClient _client;
        private IAutoRole _autoRole;

        public async Task Initialize(DiscordSocketClient client, IServiceProvider services)
        {
            _client = client;
            _autoRole = DatabaseFactory.GenerateAutoRole();
            _client.GuildMemberUpdated += GuildMemberUpdated;
        }

        private async Task GuildMemberUpdated(SocketGuildUser oldState, SocketGuildUser newState)
        {
            if (oldState.Activity?.Name == newState.Activity?.Name) return;
            RemoveRole(oldState);
            AddRole(newState);
        }

        private async Task RemoveRole(SocketGuildUser user)
        {
            if (user.Activity == null) return;
            if (user.Activity.Type != ActivityType.Playing) return;
            var roleData = _autoRole.GetData(user.Guild.Id);
            var game = user.Activity.Name;
            var roles = user.Roles.Where(r => r.Name.StartsWith(roleData.AutoPrefix, StringComparison.OrdinalIgnoreCase) && r.Name.ContainsIgnoreCase(game));
            foreach (var role in roles)
            {
                await user.RemoveRoleAsync(role);
                LogsHandler.Instance.Log("Roles", user.Guild, $"{user.Nickname()} lost role `{role.Name}`.");
            }
        }

        private async Task AddRole(SocketGuildUser user)
        {
            if (user.Activity == null) return;
            if (user.Activity.Type != ActivityType.Playing) return;
            var roleData = _autoRole.GetData(user.Guild.Id);
            var game = user.Activity.Name;
            foreach (var role in user.Guild.Roles)
            {
                if (!role.Name.ContainsIgnoreCase(game)) continue;
                if (user.Roles.Contains(role)) continue;
                if (!role.Name.StartsWith(roleData.AutoPrefix, StringComparison.OrdinalIgnoreCase) &&
                    !role.Name.StartsWith(roleData.PermaPrefix, StringComparison.OrdinalIgnoreCase)) continue;
                await user.AddRoleAsync(role);
                LogsHandler.Instance.Log("Roles", user.Guild, $"{user.Nickname()} got role `{role.Name}`.");
            }
        }
    }
}