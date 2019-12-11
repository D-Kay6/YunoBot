using Discord.Commands;
using System;
using System.Threading.Tasks;
using Discord;
using IRole = IDal.Interfaces.Database.IRole;

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
        public class PermaRolePrefixModule : ModuleBase<SocketCommandContext>
        {
            private IRole _role;
            private Localization.Localization _lang;

            public PermaRolePrefixModule(IRole role)
            {
                _role = role;
            }

            protected override void BeforeExecute(CommandInfo command)
            {
                _lang = new Localization.Localization(Context.Guild.Id);
                base.BeforeExecute(command);
            }

            [Command]
            public async Task DefaultPermaRolePrefix()
            {
                await ReplyAsync(_lang.GetMessage("Permarole prefix default", _role.GetPermaPrefix(Context.Guild.Id)));
            }

            [Command("set")]
            public async Task PermaRolePrefixSet([Remainder] string message)
            {
                if (message.Equals(_role.GetPermaPrefix(Context.Guild.Id), StringComparison.OrdinalIgnoreCase))
                {
                    await ReplyAsync(_lang.GetMessage("Invalid ar/pr prefix"));
                    return;
                }
                _role.SetPermaPrefix(Context.Guild.Id, message);
                await ReplyAsync(_lang.GetMessage("Permarole prefix set", message));
            }
        }
    }
}