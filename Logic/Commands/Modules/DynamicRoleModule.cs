using Core.Entity;
using Core.Enum;
using Discord;
using Discord.Commands;
using IDal.Database;
using Logic.Commands.TypeReaders;
using Logic.Services;
using System.Threading.Tasks;

namespace Logic.Commands.Modules
{
    [Alias("dr")]
    [Group("dynamicrole")]
    [RequireOwner]
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

            var dynamicRole = await _dynamic.Get(Context.Guild.Id, status);
            if (dynamicRole != null)
            {
                var result = dynamicRole.Roles.Add(new DynamicRoleData
                {
                    RoleId = role.Id,
                    DynamicRoleId = dynamicRole.Id
                });
                if (!result)
                {
                    await ReplyAsync(_localization.GetMessage("Dynamic role invalid add role"));
                    return;
                }

                await _dynamic.Save(dynamicRole);
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
                    DynamicRoleId = dynamicRole.Id
                });
            }

            switch (type)
            {
                case AutomationType.Temporary:
                    await ReplyAsync(_localization.GetMessage("Dynamic role add temp finish", role.Name, dynamicRole.Status));
                    break;
                case AutomationType.Permanent:
                    await ReplyAsync(_localization.GetMessage("Dynamic role add perm finish", role.Name, dynamicRole.Status));
                    break;
            }
        }

        [Command("remove")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DynamicRoleRemove(string status)
        {
        }

        [Command("remove")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DynamicRoleRemove(string status, AutomationType type)
        {
        }

        [Command("remove")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DynamicRoleRemove(string status, AutomationType type, IRole role)
        {
        }

        [Command("list")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DynamicRoleList()
        {
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