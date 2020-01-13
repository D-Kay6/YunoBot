using Discord;
using Discord.Commands;
using IDal.Database;
using Logic.Services;
using System;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Alias("ar")]
    [Group("autorole")]
    public class AutoRoleModule : ModuleBase<SocketCommandContext>
    {
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;

        public AutoRoleModule(IDbLanguage language, LocalizationService localization)
        {
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

        [Group("prefix")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public class AutoRolePrefixModule : ModuleBase<SocketCommandContext>
        {
            private IDbRole _role;
            private IDbLanguage _language;
            private LocalizationService _localization;

            public AutoRolePrefixModule(IDbRole role, IDbLanguage language, LocalizationService localization)
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
            public async Task DefaultAutoRolePrefix()
            {
                await ReplyAsync(_localization.GetMessage("Autorole prefix default", await _role.GetAutoPrefix(Context.Guild.Id)));
            }

            [Command("set")]
            public async Task AutoRolePrefixSet([Remainder] string message)
            {
                if (message.Equals(await _role.GetAutoPrefix(Context.Guild.Id), StringComparison.OrdinalIgnoreCase))
                {
                    await ReplyAsync(_localization.GetMessage("Invalid ar/pr prefix"));
                    return;
                }
                await _role.SetAutoPrefix(Context.Guild.Id, message);
                await ReplyAsync(_localization.GetMessage("Autorole prefix set", message));
            }
        }

        [Group("ignore")]
        public class AutoRoleIgnoreModule : ModuleBase<SocketCommandContext>
        {
            private IDbRole _role;
            private IDbLanguage _language;
            private LocalizationService _localization;

            public AutoRoleIgnoreModule(IDbRole role, IDbLanguage language, LocalizationService localization)
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
                var activity = await _role.IsIgnoringRoles(Context.Guild.Id, Context.User.Id) ? "on" : "off";
                await ReplyAsync(_localization.GetMessage($"RoleIgnore default {activity}"));
            }

            [Command("on")]
            public async Task AutoRoleIgnoreOn()
            {
                if (await _role.IsIgnoringRoles(Context.Guild.Id, Context.User.Id))
                {
                    await ReplyAsync(_localization.GetMessage($"RoleIgnore is on"));
                    return;
                }

                await _role.AddIgnoringRoles(Context.Guild.Id, Context.User.Id);
                await ReplyAsync(_localization.GetMessage($"RoleIgnore turned on"));
            }

            [Command("off")]
            public async Task AutoRoleIgnoreOff()
            {
                if (!await _role.IsIgnoringRoles(Context.Guild.Id, Context.User.Id))
                {
                    await ReplyAsync(_localization.GetMessage($"RoleIgnore is off"));
                    return;
                }

                await _role.RemoveIgnoringRoles(Context.Guild.Id, Context.User.Id);
                await ReplyAsync(_localization.GetMessage($"RoleIgnore turned off"));
            }
        }
    }
}