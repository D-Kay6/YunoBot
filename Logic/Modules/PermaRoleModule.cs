using Discord.Commands;
using System;
using System.Threading.Tasks;
using Discord;
using IDal.Interfaces.Database;
using IDbRole = IDal.Interfaces.Database.IDbRole;

namespace Logic.Modules
{
    [Alias("pr")]
    [Group("permarole")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class PermaRoleModule : ModuleBase<SocketCommandContext>
    {
        private IDbLanguage _language;
        private Localization.Localization _lang;

        public PermaRoleModule(IDbLanguage language)
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
        public async Task DefaultPermaRole()
        {
            await ReplyAsync(_lang.GetMessage("Permarole default"));
        }

        [Group("prefix")]
        public class PermaRolePrefixModule : ModuleBase<SocketCommandContext>
        {
            private IDbRole _role;
            private IDbLanguage _language;
            private Localization.Localization _lang;

            public PermaRolePrefixModule(IDbRole role, IDbLanguage language)
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
            public async Task DefaultPermaRolePrefix()
            {
                await ReplyAsync(_lang.GetMessage("Permarole prefix default", await _role.GetPermaPrefix(Context.Guild.Id)));
            }

            [Command("set")]
            public async Task PermaRolePrefixSet([Remainder] string message)
            {
                if (message.Equals(await _role.GetPermaPrefix(Context.Guild.Id), StringComparison.OrdinalIgnoreCase))
                {
                    await ReplyAsync(_lang.GetMessage("Invalid ar/pr prefix"));
                    return;
                }
                await _role.SetPermaPrefix(Context.Guild.Id, message);
                await ReplyAsync(_lang.GetMessage("Permarole prefix set", message));
            }
        }
    }
}