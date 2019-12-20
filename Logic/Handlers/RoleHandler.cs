using Discord.WebSocket;
using Logic.Extensions;
using Logic.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using IDbRole = IDal.Interfaces.Database.IDbRole;

namespace Logic.Handlers
{
    public class RoleHandler : BaseHandler
    {
        private IDbRole _role;

        public RoleHandler(DiscordSocketClient client, IDbRole role) : base(client)
        {
            _role = role;
        }

        public override async Task Initialize()
        {
            Client.GuildMemberUpdated += GuildMemberUpdated;
        }

        private async Task GuildMemberUpdated(SocketGuildUser oldState, SocketGuildUser newState)
        {
            if (oldState.Activity?.Name == newState.Activity?.Name) return;
            await RemoveRole(oldState);
            await AddRole(newState);
        }

        private async Task RemoveRole(SocketGuildUser user)
        {
            try
            {
                if (user.Activity == null) return;
                var roleData = await _role.GetAutoChannel(user.Guild.Id);
                var roles = user.Roles.Where(r => r.Name.StartsWith(roleData.Prefix, StringComparison.OrdinalIgnoreCase) && r.Name.ContainsIgnoreCase(user.Activity.Name));
                foreach (var role in roles)
                {
                    await user.RemoveRoleAsync(role);
                    LogService.Instance.Log("Roles", user.Guild, $"{user.Nickname()} lost role `{role.Name}`.");
                }
            }
            catch (Exception e)
            {
                LogService.Instance.Log("Crashes", $"Removing a role broke. {e.Message}");
            }
        }

        private async Task AddRole(SocketGuildUser user)
        {
            try
            {
                if (await _role.IsIgnoringRoles(user.Guild.Id, user.Id)) return;
                if (user.Activity == null) return;

                var autoPrefix = await _role.GetAutoPrefix(user.Guild.Id);
                var permaPrefix = await _role.GetPermaPrefix(user.Guild.Id);
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
            catch (Exception e)
            {
                LogService.Instance.Log("Crashes", $"Adding a role broke. {e.Message}");
            }
        }
    }
}