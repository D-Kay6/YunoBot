using Discord.Commands;
using System;
using System.Threading.Tasks;
using Discord;
using IRole = IDal.Interfaces.Database.IRole;

namespace Logic.Modules
{
    [Alias("ar")]
    [Group("autorole")]
    public class AutoRoleModule : ModuleBase<SocketCommandContext>
    {
        private Localization.Localization _lang;

        protected override void BeforeExecute(CommandInfo command)
        {
            _lang = new Localization.Localization(Context.Guild.Id);
            base.BeforeExecute(command);
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
            private IRole _role;
            private Localization.Localization _lang;

            public AutoRolePrefixModule(IRole role)
            {
                _role = role;
            }

            protected override void BeforeExecute(CommandInfo command)
            {
                _lang = new Localization.Localization(Context.Guild.Id);
                base.BeforeExecute(command);
            }

            [Command]
            public async Task DefaultAutoRolePrefix()
            {
                await ReplyAsync(_lang.GetMessage("Autorole prefix default", _role.GetAutoPrefix(Context.Guild.Id)));
            }

            [Command("set")]
            public async Task AutoRolePrefixSet([Remainder] string message)
            {
                if (message.Equals(_role.GetAutoPrefix(Context.Guild.Id), StringComparison.OrdinalIgnoreCase))
                {
                    await ReplyAsync(_lang.GetMessage("Invalid ar/pr prefix"));
                    return;
                }
                _role.SetAutoPrefix(Context.Guild.Id, message);
                await ReplyAsync(_lang.GetMessage("Autorole prefix set", message));
            }
        }

        [Group("ignore")]
        public class AutoRoleIgnoreModule : ModuleBase<SocketCommandContext>
        {
            private IRole _role;
            private Localization.Localization _lang;

            public AutoRoleIgnoreModule(IRole role)
            {
                _role = role;
            }

            protected override void BeforeExecute(CommandInfo command)
            {
                _lang = new Localization.Localization(Context.Guild.Id);
                base.BeforeExecute(command);
            }

            [Command]
            public async Task DefaultAutoRoleIgnore()
            {
                var activity = _role.IsIgnoringRoles(Context.Guild.Id, Context.User.Id) ? "on" : "off";
                await ReplyAsync(_lang.GetMessage($"RoleIgnore default {activity}"));
            }

            [Command("on")]
            public async Task AutoRoleIgnoreOn()
            {
                if (_role.IsIgnoringRoles(Context.Guild.Id, Context.User.Id))
                {
                    await ReplyAsync(_lang.GetMessage($"RoleIgnore is on"));
                    return;
                }

                _role.AddIgnoringRoles(Context.Guild.Id, Context.User.Id);
                await ReplyAsync(_lang.GetMessage($"RoleIgnore turned on"));
            }

            [Command("off")]
            public async Task AutoRoleIgnoreOff()
            {
                if (!_role.IsIgnoringRoles(Context.Guild.Id, Context.User.Id))
                {
                    await ReplyAsync(_lang.GetMessage($"RoleIgnore is off"));
                    return;
                }

                _role.RemoveIgnoringRoles(Context.Guild.Id, Context.User.Id);
                await ReplyAsync(_lang.GetMessage($"RoleIgnore turned off"));
            }
        }
    }
}