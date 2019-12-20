using Discord.Commands;
using System;
using System.Threading.Tasks;
using Discord;
using IDal.Interfaces.Database;
using IDbRole = IDal.Interfaces.Database.IDbRole;

namespace Logic.Modules
{
    [Alias("ar")]
    [Group("autorole")]
    public class AutoRoleModule : ModuleBase<SocketCommandContext>
    {
        private IDbLanguage _language;
        private Localization.Localization _lang;

        public AutoRoleModule(IDbLanguage language)
        {
            _language = language;
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            Task.WaitAll(LoadLanguage());
            base.BeforeExecute(command);
        }

        private async Task LoadLanguage()
        {
            _lang = new Localization.Localization(await _language.GetLanguage(Context.Guild.Id));
        }

        [Command]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DefaultAutoRole()
        {
            await ReplyAsync(_lang.GetMessage("Autorole default"));
        }

        [Group("prefix")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public class AutoRolePrefixModule : ModuleBase<SocketCommandContext>
        {
            private IDbRole _role;
            private IDbLanguage _language;
            private Localization.Localization _lang;

            public AutoRolePrefixModule(IDbRole role, IDbLanguage language)
            {
                _role = role;
                _language = language;
            }

            protected override void BeforeExecute(CommandInfo command)
            {
                Task.WaitAll(LoadLanguage());
                base.BeforeExecute(command);
            }

            private async Task LoadLanguage()
            {
                _lang = new Localization.Localization(await _language.GetLanguage(Context.Guild.Id));
            }

            [Command]
            public async Task DefaultAutoRolePrefix()
            {
                await ReplyAsync(_lang.GetMessage("Autorole prefix default", await _role.GetAutoPrefix(Context.Guild.Id)));
            }

            [Command("set")]
            public async Task AutoRolePrefixSet([Remainder] string message)
            {
                if (message.Equals(await _role.GetAutoPrefix(Context.Guild.Id), StringComparison.OrdinalIgnoreCase))
                {
                    await ReplyAsync(_lang.GetMessage("Invalid ar/pr prefix"));
                    return;
                }
                await _role.SetAutoPrefix(Context.Guild.Id, message);
                await ReplyAsync(_lang.GetMessage("Autorole prefix set", message));
            }
        }

        [Group("ignore")]
        public class AutoRoleIgnoreModule : ModuleBase<SocketCommandContext>
        {
            private IDbRole _role;
            private IDbLanguage _language;
            private Localization.Localization _lang;

            public AutoRoleIgnoreModule(IDbRole role, IDbLanguage language)
            {
                _role = role;
                _language = language;
            }

            protected override void BeforeExecute(CommandInfo command)
            {
                Task.WaitAll(LoadLanguage());
                base.BeforeExecute(command);
            }

            private async Task LoadLanguage()
            {
                _lang = new Localization.Localization(await _language.GetLanguage(Context.Guild.Id));
            }

            [Command]
            public async Task DefaultAutoRoleIgnore()
            {
                var activity = await _role.IsIgnoringRoles(Context.Guild.Id, Context.User.Id) ? "on" : "off";
                await ReplyAsync(_lang.GetMessage($"RoleIgnore default {activity}"));
            }

            [Command("on")]
            public async Task AutoRoleIgnoreOn()
            {
                if (await _role.IsIgnoringRoles(Context.Guild.Id, Context.User.Id))
                {
                    await ReplyAsync(_lang.GetMessage($"RoleIgnore is on"));
                    return;
                }

                await _role.AddIgnoringRoles(Context.Guild.Id, Context.User.Id);
                await ReplyAsync(_lang.GetMessage($"RoleIgnore turned on"));
            }

            [Command("off")]
            public async Task AutoRoleIgnoreOff()
            {
                if (!await _role.IsIgnoringRoles(Context.Guild.Id, Context.User.Id))
                {
                    await ReplyAsync(_lang.GetMessage($"RoleIgnore is off"));
                    return;
                }

                await _role.RemoveIgnoringRoles(Context.Guild.Id, Context.User.Id);
                await ReplyAsync(_lang.GetMessage($"RoleIgnore turned off"));
            }
        }
    }
}