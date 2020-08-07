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
        private readonly RoleService _role;

        public RoleHandler(DiscordShardedClient client, LogsService logs, RoleService role) : base(client, logs)
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
                var roleData = await _role.LoadAuto(user.Guild.Id);
                var roles = user.Roles.Where(r =>
                    r.Name.StartsWith(roleData.Prefix, StringComparison.OrdinalIgnoreCase) &&
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

                var auto = await _role.LoadAuto(user.Guild.Id);
                var perma = await _role.LoadPerma(user.Guild.Id);
                foreach (var role in user.Guild.Roles)
                {
                    if (!role.Name.ContainsIgnoreCase(user.Activity.Name)) continue;
                    if (user.Roles.Contains(role)) continue;
                    if (!role.Name.StartsWith(auto.Prefix, StringComparison.OrdinalIgnoreCase) &&
                        !role.Name.StartsWith(perma.Prefix, StringComparison.OrdinalIgnoreCase)) continue;
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