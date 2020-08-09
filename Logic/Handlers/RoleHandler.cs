using Core.Entity;
using Discord.WebSocket;
using Logic.Extensions;
using Logic.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Handlers
{
    public class RoleHandler : BaseHandler
    {
        private readonly DynamicRoleService _role;

        public RoleHandler(DiscordShardedClient client, LogsService logs, DynamicRoleService role) : base(client, logs)
        {
            _role = role;
        }

        public override Task Initialize()
        {
            base.Initialize();
            Client.GuildMemberUpdated += GuildMemberUpdated;
            return Task.CompletedTask;
        }

        private async Task GuildMemberUpdated(SocketGuildUser oldState, SocketGuildUser newState)
        {
#if DEBUG
            if (!oldState.Id.Equals(255453041531158538)) return;
#endif
            if (oldState.Activity?.Name == newState.Activity?.Name) return;
            await RemoveRole(oldState).ConfigureAwait(false);
            await AddRole(newState).ConfigureAwait(false);
        }

        private async Task RemoveRole(SocketGuildUser user)
        {
            try
            {
                if (user.Activity == null) return;
                DynamicRole roleData = null;
                var roles = user.Roles.Where(r =>
                    r.Name.StartsWith(roleData.Status, StringComparison.OrdinalIgnoreCase) &&
                    r.Name.ContainsIgnoreCase(user.Activity.Name));
                foreach (var role in roles)
                {
                    await user.RemoveRoleAsync(role);
                    await Logs.Write("Roles", $"{user.Nickname()} lost role `{role.Name}`.", user.Guild);
                }
            }
            catch (Exception e)
            {
                await Logs.Write("Crashes", $"Removing a role broke.", e);
            }
        }

        private async Task AddRole(SocketGuildUser user)
        {
            try
            {
                if (await _role.IsRoleIgnore(user.Guild.Id, user.Id)) return;
                if (user.Activity == null) return;

                DynamicRole auto = null;
                DynamicRole perma = null;
                foreach (var role in user.Guild.Roles)
                {
                    if (!role.Name.ContainsIgnoreCase(user.Activity.Name)) continue;
                    if (user.Roles.Contains(role)) continue;
                    if (!role.Name.StartsWith(auto.Status, StringComparison.OrdinalIgnoreCase) &&
                        !role.Name.StartsWith(perma.Status, StringComparison.OrdinalIgnoreCase)) continue;
                    await user.AddRoleAsync(role);
                    await Logs.Write("Roles", $"{user.Nickname()} got role `{role.Name}`.", user.Guild);
                }
            }
            catch (Exception e)
            {
                await Logs.Write("Crashes", $"Adding a role broke.", e);
            }
        }
    }
}