using Discord;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Threading.Tasks;
using Yuno.Main.Extentions;
using Yuno.Main.Logging;

namespace Yuno.Main.AutoRole
{
    public class RoleHandler
    {
        private DiscordSocketClient _client;

        public async Task Initialize(DiscordSocketClient client, IServiceProvider services)
        {
            this._client = client;
            _client.GuildMemberUpdated += GuildMemberUpdated;
        }

        private async Task GuildMemberUpdated(SocketGuildUser oldState, SocketGuildUser newState)
        {
            if (oldState.Activity?.Name == newState.Activity?.Name) return;
            if (oldState.Activity != null) RemoveRole(oldState);
            if (newState.Activity != null) AddRole(newState);
        }

        private async void RemoveRole(SocketGuildUser user)
        {
            if (user.Activity.Type != ActivityType.Playing) return;
            var autoRole = Logic.AutoRole.Load(user.Guild.Id);
            var game = user.Activity.Name;
            var roles = user.Roles.Where(r => autoRole.IsAutoRole(r) && r.Name.ContainsIgnoreCase(game));
            await user.RemoveRolesAsync(roles);
        }

        private async void AddRole(SocketGuildUser user)
        {
            if (user.Activity.Type != ActivityType.Playing) return;
            var autoRole = Logic.AutoRole.Load(user.Guild.Id);
            var game = user.Activity.Name;

            var rolesA = user.Guild.Roles.Where(r => autoRole.IsAutoRole(r) && r.Name.ContainsIgnoreCase(game));
            await user.AddRolesAsync(rolesA);

            var rolesP = user.Guild.Roles.Where(r => autoRole.IsPermaRole(r) && r.Name.ContainsIgnoreCase(game));
            await user.AddRolesAsync(rolesP);
        }
    }
}