using Core.Enum;
using Discord;
using Discord.WebSocket;
using Logic.Extensions;
using Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Handlers
{
    public class DynamicRoleHandler : BaseHandler
    {
        private readonly DynamicRoleService _role;

        public DynamicRoleHandler(DiscordShardedClient client, LogsService logs, DynamicRoleService role) : base(client, logs)
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
            var removed = oldState.Activities.Where(x => x.Type == ActivityType.Playing)
                .Except(newState.Activities.Where(x => x.Type == ActivityType.Playing));
            await RemoveRole(oldState, removed).ConfigureAwait(false);

            var added = newState.Activities.Where(x => x.Type == ActivityType.Playing)
                .Except(oldState.Activities.Where(x => x.Type == ActivityType.Playing));
            await AddRole(newState, added).ConfigureAwait(false);
        }

        private async Task RemoveRole(SocketGuildUser user, IEnumerable<IActivity> activities)
        {
            foreach (var activity in activities)
            {
                try
                {
                    var dynamicRoles = await _role.Find(user.Guild.Id, activity.Name, AutomationType.Temporary);
                    foreach (var dynamicRole in dynamicRoles)
                    {
                        try
                        {
                            foreach (var roleData in dynamicRole.Roles.ToList())
                            {
                                try
                                {
                                    var role = user.Guild.GetRole(roleData.RoleId);
                                    if (role == null)
                                    {
                                        await _role.Delete(roleData);
                                        continue;
                                    }
                                    await user.RemoveRoleAsync(role);
                                    await Logs.Write("Roles", $"{user.Nickname()} lost role `{role.Name}`.", user.Guild);
                                }
                                catch (Exception e)
                                {

                                }
                            }
                        }
                        catch (Exception e)
                        {
                            await Logs.Write("Crashes", $"Removing dynamic role {dynamicRole.Status} ({dynamicRole.Type}) broke.", e);
                        }
                    }
                }
                catch (Exception e)
                {
                    await Logs.Write("Crashes", $"Removing activity {activity.Name} broke.", e);
                }
            }
        }

        private async Task AddRole(SocketGuildUser user, IEnumerable<IActivity> activities)
        {
            foreach (var activity in activities)
            {
                try
                {
                    var dynamicRoles = await _role.Find(user.Guild.Id, activity.Name);
                    foreach (var dynamicRole in dynamicRoles)
                    {
                        try
                        {
                            foreach (var roleData in dynamicRole.Roles.ToList())
                            {
                                try
                                {
                                    var role = user.Guild.GetRole(roleData.RoleId);
                                    if (role == null)
                                    {
                                        await _role.Delete(roleData);
                                        continue;
                                    }
                                    await user.AddRoleAsync(role);
                                    await Logs.Write("Roles", $"{user.Nickname()} got role `{role.Name}`.", user.Guild);
                                }
                                catch (Exception e)
                                {
                                    await Logs.Write("Crashes", $"Adding role {roleData.RoleId} broke.", e, user.Guild);
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            await Logs.Write("Crashes", $"Adding dynamic role {dynamicRole.Status} ({dynamicRole.Type}) broke.", e, user.Guild);
                        }
                    }
                }
                catch (Exception e)
                {
                    await Logs.Write("Crashes", $"Adding activity {activity.Name} broke.", e, user.Guild);
                }
            }
        }
    }
}