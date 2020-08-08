using Core.Entity;
using Core.Enum;
using Discord;
using Discord.Commands;
using IDal.Database;
using Logic.Exceptions;
using Logic.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Alias("dr")]
    [Group("dynamicrole")]
    [RequireOwner]
    public class DynamicRoleModule : ModuleBase<ShardedCommandContext>
    {
        private readonly DynamicRoleService _dynamic;
        private readonly RoleService _role;
        private readonly LogsService _logs;
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;

        public DynamicRoleModule(DynamicRoleService dynamic, RoleService role, LogsService logs, IDbLanguage language, LocalizationService localization)
        {
            _dynamic = dynamic;
            _role = role;
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
        public async Task DynamicRoleAdd(string status, string type, IRole role)
        {
            if (string.IsNullOrWhiteSpace(status)) return;
            if (string.IsNullOrWhiteSpace(type)) return;

            AutomationType automationType;
            switch (type.ToLower())
            {
                case "temp":
                case "temporary":
                case "auto":
                case "automatic":
                    automationType = AutomationType.Temporary;
                    break;
                case "perm":
                case "perma":
                case "permanent":
                    automationType = AutomationType.Permanent;
                    break;
                default:
                    return;
            }

            var dynamicRole = await _dynamic.Get(Context.Guild.Id, status);
            if (dynamicRole != null)
            {
                if (dynamicRole.Roles.Any(x => x.RoleId == role.Id))
                {
                    return;
                }
                
                dynamicRole.Roles.Add(new DynamicRoleData
                {
                    RoleId = role.Id
                });
                return;
            }

            dynamicRole = new DynamicRole
            {
                ServerId = Context.Guild.Id,
                Type = automationType,
                Status = status
            };
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