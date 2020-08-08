using Core.Entity;
using Discord;
using Discord.Commands;
using IDal.Database;
using Logic.Exceptions;
using Logic.Services;
using System;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Alias("ar")]
    [Group("autorole")]
    public class AutoRoleModule : ModuleBase<ShardedCommandContext>
    {
        private readonly DynamicRoleService _role;
        private readonly LogsService _logs;
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;

        public AutoRoleModule(DynamicRoleService role, LogsService logs, IDbLanguage language, LocalizationService localization)
        {
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
        public async Task DefaultAutoRole()
        {
            await ReplyAsync(_localization.GetMessage("Autorole default"));
        }

        [Group("ignore")]
        public class AutoRoleIgnoreModule : ModuleBase<ShardedCommandContext>
        {
            private readonly IDbLanguage _language;
            private readonly LocalizationService _localization;
            private readonly DynamicRoleService _role;

            public AutoRoleIgnoreModule(DynamicRoleService role, IDbLanguage language, LocalizationService localization)
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
            public async Task DefaultAutoRoleIgnore()
            {
                var activity = await _role.IsRoleIgnore(Context.Guild.Id, Context.User.Id) ? "on" : "off";
                await ReplyAsync(_localization.GetMessage($"RoleIgnore default {activity}"));
            }

            [Command("on")]
            public async Task AutoRoleIgnoreOn()
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
            public async Task AutoRoleIgnoreOff()
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