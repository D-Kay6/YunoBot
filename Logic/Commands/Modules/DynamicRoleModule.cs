using Core.Entity;
using Core.Enum;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using IDal.Database;
using Logic.Commands.TypeReaders;
using Logic.Exceptions;
using Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Commands.Modules
{
    [Alias("dr")]
    [Group("dynamicrole")]
    public class DynamicRoleModule : ModuleBase<ShardedCommandContext>
    {
        private readonly DynamicRoleService _dynamic;
        private readonly LogsService _logs;
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;

        public DynamicRoleModule(DynamicRoleService dynamic, LogsService logs, IDbLanguage language, LocalizationService localization)
        {
            _dynamic = dynamic;
            _logs = logs;
            _language = language;
            _localization = localization;
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            Task.WaitAll(Prepare());
            base.BeforeExecute(command);
        }

        private async Task Prepare()
        {
            await _localization.Load(await _language.GetLanguage(Context.Guild.Id));
        }

        [Command]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DefaultDynamicRole()
        {
            await ReplyAsync(_localization.GetMessage("dynamic role default"));
        }

        [Command("add")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DynamicRoleAdd(string status, [OverrideTypeReader(typeof(AutomationTypeReader))] AutomationType type, IRole role)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                await ReplyAsync(_localization.GetMessage("Dynamic role invalid add status"));
                return;
            }

            var dynamicRole = await _dynamic.Get(Context.Guild.Id, status, type);
            if (dynamicRole != null)
            {
                try
                {
                    await _dynamic.Save(new DynamicRoleData
                    {
                        RoleId = role.Id,
                        DynamicRoleId = dynamicRole.Id,
                        DynamicRole = dynamicRole
                    });
                }
                catch (DataExistsException e)
                {

                    await ReplyAsync(_localization.GetMessage("Dynamic role invalid add role"));
                    return;
                }
            }
            else
            {
                dynamicRole = new DynamicRole
                {
                    ServerId = Context.Guild.Id,
                    Type = type,
                    Status = status.ToLower()
                };
                await _dynamic.Save(dynamicRole);
                await _dynamic.Save(new DynamicRoleData
                {
                    RoleId = role.Id,
                    DynamicRoleId = dynamicRole.Id,
                    DynamicRole = dynamicRole
                });
            }

            switch (type)
            {
                case AutomationType.Temporary:
                    await ReplyAsync(_localization.GetMessage("Dynamic role add temp", role.Name, dynamicRole.Status));
                    break;
                case AutomationType.Permanent:
                    await ReplyAsync(_localization.GetMessage("Dynamic role add perm", role.Name, dynamicRole.Status));
                    break;
            }
        }

        [Priority(1)]
        [Command("remove all")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DynamicRoleRemove()
        {
            var roles = await _dynamic.Get(Context.Guild.Id);
            if (roles == null || !roles.Any())
            {
                await ReplyAsync(_localization.GetMessage("Dynamic role invalid remove amount"));
                return;
            }

            try
            {
                await _dynamic.Delete(roles);
            }
            catch (Exception e)
            {
                await _logs.Write("Crashes", "An unhandled exception has occured.", e);
                await ReplyAsync(_localization.GetMessage("General error"));
                return;
            }

            await ReplyAsync(_localization.GetMessage("Dynamic role remove all"));
        }

        [Command("remove")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DynamicRoleRemove(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                await ReplyAsync(_localization.GetMessage("Dynamic role invalid remove status"));
                return;
            }

            var roles = await _dynamic.Get(Context.Guild.Id, status);
            if (roles == null || !roles.Any())
            {
                await ReplyAsync(_localization.GetMessage("Dynamic role invalid remove amount"));
                return;
            }

            try
            {
                await _dynamic.Delete(roles);
            }
            catch (Exception e)
            {
                await _logs.Write("Crashes", "An unhandled exception has occured.", e);
                await ReplyAsync(_localization.GetMessage("General error"));
                return;
            }

            await ReplyAsync(_localization.GetMessage("Dynamic role remove game all", status));
        }

        [Command("remove")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DynamicRoleRemove([OverrideTypeReader(typeof(AutomationTypeReader))] AutomationType type)
        {
            var roles = await _dynamic.Get(Context.Guild.Id, type: type);
            if (roles == null || !roles.Any())
            {
                await ReplyAsync(_localization.GetMessage("Dynamic role invalid remove amount"));
                return;
            }

            try
            {
                await _dynamic.Delete(roles);
            }
            catch (Exception e)
            {
                await _logs.Write("Crashes", "An unhandled exception has occured.", e);
                await ReplyAsync(_localization.GetMessage("General error"));
                return;
            }

            switch (type)
            {
                case AutomationType.Temporary:
                    await ReplyAsync(_localization.GetMessage("Dynamic role remove temp"));
                    break;
                case AutomationType.Permanent:
                    await ReplyAsync(_localization.GetMessage("Dynamic role remove perm"));
                    break;
            }
        }

        [Command("remove")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DynamicRoleRemove(string status, [OverrideTypeReader(typeof(AutomationTypeReader))] AutomationType type)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                await ReplyAsync(_localization.GetMessage("Dynamic role invalid remove status"));
                return;
            }

            var role = await _dynamic.Get(Context.Guild.Id, status, type);
            if (role == null)
            {
                await ReplyAsync(_localization.GetMessage("Dynamic role invalid remove amount"));
                return;
            }

            try
            {
                await _dynamic.Delete(role);
            }
            catch (Exception e)
            {
                await _logs.Write("Crashes", "An unhandled exception has occured.", e);
                await ReplyAsync(_localization.GetMessage("General error"));
                return;
            }

            switch (type)
            {
                case AutomationType.Temporary:
                    await ReplyAsync(_localization.GetMessage("Dynamic role remove game temp", status));
                    break;
                case AutomationType.Permanent:
                    await ReplyAsync(_localization.GetMessage("Dynamic role remove game perm", status));
                    break;
            }
        }

        [Command("remove")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DynamicRoleRemove(string status, [OverrideTypeReader(typeof(AutomationTypeReader))] AutomationType type, IRole role)
        {
            if (string.IsNullOrWhiteSpace(status))
            {
                await ReplyAsync(_localization.GetMessage("Dynamic role invalid remove status"));
                return;
            }

            var dynamicRole = await _dynamic.Get(Context.Guild.Id, status, type);
            if (dynamicRole == null)
            {
                await ReplyAsync(_localization.GetMessage("Dynamic role invalid remove amount"));
                return;
            }

            var dynamicData = dynamicRole.Roles.FirstOrDefault(x => x.RoleId == role.Id);
            if (dynamicData == null)
            {
                await ReplyAsync(_localization.GetMessage("Dynamic role invalid remove role", dynamicRole.Status));
                return;
            }

            try
            {
                await _dynamic.Delete(dynamicData);
            }
            catch (Exception e)
            {
                await _logs.Write("Crashes", "An unhandled exception has occured.", e);
                await ReplyAsync(_localization.GetMessage("General error"));
                return;
            }

            switch (type)
            {
                case AutomationType.Temporary:
                    await ReplyAsync(_localization.GetMessage("Dynamic role remove game role temp", role.Name, status));
                    break;
                case AutomationType.Permanent:
                    await ReplyAsync(_localization.GetMessage("Dynamic role remove game role perm", role.Name, status));
                    break;
            }
        }

        [Command("list")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DynamicRoleList()
        {
            var embedBuilder = new EmbedBuilder();
            var dynamicRoles = await _dynamic.Get(Context.Guild.Id);
            if (!dynamicRoles.Any())
            {
                await ReplyAsync(_localization.GetMessage("Dynamic role invalid list"));
                return;
            }

            foreach (var dynamicRole in dynamicRoles.OrderBy(x => x.Status).ThenBy(x => x.Type))
            {
                if (!dynamicRole.Roles.Any()) continue;

                var name = $"{dynamicRole.Status} ({dynamicRole.Type})";
                var roles = new List<SocketRole>();
                foreach (var roleData in dynamicRole.Roles.ToList())
                {
                    var role = Context.Guild.GetRole(roleData.RoleId);
                    if (role == null)
                    {
                        await _dynamic.Delete(roleData);
                        continue;
                    }
                    roles.Add(role);
                }

                embedBuilder.AddField(name, string.Join("\r\n", roles.Select(x => x.Name)));
            }

            await ReplyAsync(embed: embedBuilder.Build());
        }

        [Group("ignore")]
        public class DynamicRoleIgnoreModule : ModuleBase<ShardedCommandContext>
        {
            private readonly IDbLanguage _language;
            private readonly LocalizationService _localization;
            private readonly DynamicRoleService _role;

            public DynamicRoleIgnoreModule(DynamicRoleService role, IDbLanguage language, LocalizationService localization)
            {
                _role = role;
                _language = language;
                _localization = localization;
            }

            protected override void BeforeExecute(CommandInfo command)
            {
                Task.WaitAll(Prepare());
                base.BeforeExecute(command);
            }

            private async Task Prepare()
            {
                await _localization.Load(await _language.GetLanguage(Context.Guild.Id));
            }

            [Command]
            public async Task DefaultDynamicRoleIgnore()
            {
                var activity = await _role.IsRoleIgnore(Context.Guild.Id, Context.User.Id) ? "on" : "off";
                await ReplyAsync(_localization.GetMessage($"RoleIgnore default {activity}"));
            }

            [Command("on")]
            public async Task DynamicRoleIgnoreOn()
            {
                if (await _role.IsRoleIgnore(Context.Guild.Id, Context.User.Id))
                {
                    await ReplyAsync(_localization.GetMessage("RoleIgnore is on"));
                    return;
                }

                await _role.AddRoleIgnore(Context.Guild.Id, Context.User.Id);
                await ReplyAsync(_localization.GetMessage("RoleIgnore turned on"));
            }

            [Command("off")]
            public async Task DynamicRoleIgnoreOff()
            {
                if (!await _role.IsRoleIgnore(Context.Guild.Id, Context.User.Id))
                {
                    await ReplyAsync(_localization.GetMessage("RoleIgnore is off"));
                    return;
                }

                await _role.RemoveRoleIgnore(Context.Guild.Id, Context.User.Id);
                await ReplyAsync(_localization.GetMessage("RoleIgnore turned off"));
            }
        }
    }
}