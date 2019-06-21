using DalFactory;
using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Alias("pr")]
    [Group("permarole")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class PermaRoleModule : ModuleBase<SocketCommandContext>
    {
        private Localization.Localization _lang;

        protected override void BeforeExecute(CommandInfo command)
        {
            _lang = new Localization.Localization(Context.Guild.Id);
            base.BeforeExecute(command);
        }

        [Command]
        public async Task DefaultPermaRole()
        {
            await ReplyAsync(_lang.GetMessage("Permarole default"));
        }

        [Group("prefix")]
        public class PermaChannelPrefixModule : ModuleBase<SocketCommandContext>
        {
            private Localization.Localization _lang;

            protected override void BeforeExecute(CommandInfo command)
            {
                _lang = new Localization.Localization(Context.Guild.Id);
                base.BeforeExecute(command);
            }

            [Command]
            public async Task DefaultPermaRolePrefix()
            {
                var autoRole = DatabaseFactory.GenerateAutoRole();
                await ReplyAsync(_lang.GetMessage("Permarole prefix default", autoRole.GetData(Context.Guild.Id).PermaPrefix));
            }

            [Command("set")]
            public async Task PermaRolePrefixSet([Remainder] string message)
            {
                var autoRole = DatabaseFactory.GenerateAutoRole();
                var data = autoRole.GetData(Context.Guild.Id);
                if (message.Equals(data.AutoPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    await ReplyAsync(_lang.GetMessage("Invalid ar/pr prefix"));
                    return;
                }
                autoRole.SetPermaPrefix(Context.Guild.Id, message);
                await ReplyAsync(_lang.GetMessage("Permarole prefix set", message));
            }
        }
    }
}