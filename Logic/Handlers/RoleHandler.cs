﻿using Discord;
using Discord.WebSocket;
using Logic.Data;
using Logic.Extentions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Handlers
{
    public class RoleHandler
    {
        private DiscordSocketClient _client;

        public async Task Initialize(DiscordSocketClient client, IServiceProvider services)
        {
            _client = client;
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
            var autoRole = AutoRole.Load(user.Guild.Id);
            var game = user.Activity.Name;
            var userName = user.Nickname ?? user.Username ?? "User";
            var roles = user.Roles.Where(r => autoRole.IsAutoRole(r) && r.Name.ContainsIgnoreCase(game));
            if (roles.Any())
            {
                await user.RemoveRolesAsync(roles);
                LogsHandler.Instance.Log("Roles", user.Guild, $"{userName} lost roles '{string.Join(" ", roles.Select(r => r.Name))}.");
            }
        }

        private async Task AddRole(SocketGuildUser user)
        {
            if (user.Activity == null) return;
            if (user.Activity.Type != ActivityType.Playing) return;
            var autoRole = AutoRole.Load(user.Guild.Id);
            var game = user.Activity.Name;

            var userName = user.Nickname ?? user.Username ?? "User";
            var rolesA = user.Guild.Roles.Where(r => autoRole.IsAutoRole(r) && r.Name.ContainsIgnoreCase(game));
            if (rolesA.Any())
            {
                await user.AddRolesAsync(rolesA);
                LogsHandler.Instance.Log("Roles", user.Guild, $"{userName} got roles '{string.Join(" ", rolesA.Select(r => r.Name))}.");
            }

            var rolesP = user.Guild.Roles.Where(r => autoRole.IsPermaRole(r) && r.Name.ContainsIgnoreCase(game));
            if (rolesP.Any())
            {
                await user.AddRolesAsync(rolesP);
                LogsHandler.Instance.Log("Roles", user.Guild, $"{userName} got roles '{string.Join(" ", rolesP.Select(r => r.Name))}.");
            }
        }
    }
}