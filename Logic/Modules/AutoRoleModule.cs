using DalFactory;
using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

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
        public class AutoChannelPrefixModule : ModuleBase<SocketCommandContext>
        {
            private Localization.Localization _lang;

            protected override void BeforeExecute(CommandInfo command)
            {
                _lang = new Localization.Localization(Context.Guild.Id);
                base.BeforeExecute(command);
            }

            [Command]
            public async Task DefaultAutoRolePrefix()
            {
                var autoRole = DatabaseFactory.GenerateAutoRole();
                await ReplyAsync(_lang.GetMessage("Autorole prefix default", autoRole.GetData(Context.Guild.Id).AutoPrefix));
            }

            [Command("set")]
            public async Task AutoRolePrefixSet([Remainder] string message)
            {
                var autoRole = DatabaseFactory.GenerateAutoRole();
                var data = autoRole.GetData(Context.Guild.Id);
                if (message.Equals(data.PermaPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    await ReplyAsync(_lang.GetMessage("Invalid ar/pr prefix"));
                    return;
                }
                autoRole.SetAutoPrefix(Context.Guild.Id, message);
                await ReplyAsync(_lang.GetMessage("Autorole prefix set", message));
            }
        }

        [Group("ignore")]
        public class AutoChannelIgnoreModule : ModuleBase<SocketCommandContext>
        {
            private Localization.Localization _lang;

            protected override void BeforeExecute(CommandInfo command)
            {
                _lang = new Localization.Localization(Context.Guild.Id);
                base.BeforeExecute(command);
            }

            [Command]
            public async Task DefaultAutoRoleIgnore()
            {
                var autoRole = DatabaseFactory.GenerateAutoRole();
                var activity = autoRole.IsRoleIgnore(Context.Guild.Id, Context.User.Id) ? "on" : "off";
                await ReplyAsync(_lang.GetMessage($"RoleIgnore default {activity}"));
            }

            [Command("on")]
            public async Task AutoRoleIgnoreOn()
            {
                var autoRole = DatabaseFactory.GenerateAutoRole();
                if (autoRole.IsRoleIgnore(Context.Guild.Id, Context.User.Id))
                {
                    await ReplyAsync(_lang.GetMessage($"RoleIgnore is on"));
                    return;
                }

                autoRole.AddRoleIgnore(Context.Guild.Id, Context.User.Id);
                await ReplyAsync(_lang.GetMessage($"RoleIgnore turned on"));
            }

            [Command("off")]
            public async Task AutoRoleIgnoreOff()
            {
                var autoRole = DatabaseFactory.GenerateAutoRole();
                if (!autoRole.IsRoleIgnore(Context.Guild.Id, Context.User.Id))
                {
                    await ReplyAsync(_lang.GetMessage($"RoleIgnore is off"));
                    return;
                }

                autoRole.RemoveRoleIgnore(Context.Guild.Id, Context.User.Id);
                await ReplyAsync(_lang.GetMessage($"RoleIgnore turned off"));
            }
        }
    }
}