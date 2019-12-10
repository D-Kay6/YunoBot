using DalFactory;
using Discord;
using Discord.WebSocket;
using IDal.Interfaces.Database;
using Logic.Extensions;
using Logic.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using IRole = IDal.Interfaces.Database.IRole;

namespace Logic.Handlers
{
    public class RoleHandler : BaseHandler
    {
        private IRole _role;

        public RoleHandler(DiscordSocketClient client, IRole role) : base(client)
        {
            _role = role;
        }

        public override async Task Initialize()
        {
            Client.GuildMemberUpdated += GuildMemberUpdated;
        }

        private async Task GuildMemberUpdated(SocketGuildUser oldState, SocketGuildUser newState)
        {
            var game = oldState.Activity as CustomStatusGame;
            if (game != null) RemoveRole(oldState);

            game = newState.Activity as CustomStatusGame;
            if (game != null) AddRole(newState);
        }

        private async Task RemoveRole(SocketGuildUser user)
        {
            if (user.Activity == null) return;
            var roleData = _role.GetAutoChannel(user.Guild.Id);
            var roles = user.Roles.Where(r => r.Name.StartsWith(roleData.Prefix, StringComparison.OrdinalIgnoreCase) && r.Name.ContainsIgnoreCase(user.Activity.Name));
            foreach (var role in roles)
            {
                await user.RemoveRoleAsync(role);
                LogService.Instance.Log("Roles", user.Guild, $"{user.Nickname()} lost role `{role.Name}`.");
            }
        }

        private async Task AddRole(SocketGuildUser user)
        {
            if (_role.IsIgnoringRoles(user.Guild.Id, user.Id)) return;
            if (user.Activity == null) return;

            var autoPrefix = _role.GetAutoPrefix(user.Guild.Id);
            var permaPrefix = _role.GetPermaPrefix(user.Guild.Id);
            foreach (var role in user.Guild.Roles)
            {
                if (!role.Name.ContainsIgnoreCase(user.Activity.Name)) continue;
                if (user.Roles.Contains(role)) continue;
                if (!role.Name.StartsWith(autoPrefix, StringComparison.OrdinalIgnoreCase) &&
                    !role.Name.StartsWith(permaPrefix, StringComparison.OrdinalIgnoreCase)) continue;
                await user.AddRoleAsync(role);
                LogService.Instance.Log("Roles", user.Guild, $"{user.Nickname()} got role `{role.Name}`.");
            }
        }
    }
}