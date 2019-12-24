using Discord;
using Discord.Commands;
using IDal.Database;
using Logic.Services;
using System;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Alias("pr")]
    [Group("permarole")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class PermaRoleModule : ModuleBase<SocketCommandContext>
    {
        private IDbLanguage _language;
        private LocalizationService _localization;

        public PermaRoleModule(IDbLanguage language, LocalizationService localization)
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
        public async Task DefaultPermaRole()
        {
            await ReplyAsync(_localization.GetMessage("Permarole default"));
        }

        [Group("prefix")]
        public class PermaRolePrefixModule : ModuleBase<SocketCommandContext>
        {
            private IDbRole _role;
            private IDbLanguage _language;
            private LocalizationService _localization;

            public PermaRolePrefixModule(IDbRole role, IDbLanguage language, LocalizationService localization)
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
            public async Task DefaultPermaRolePrefix()
            {
                await ReplyAsync(_localization.GetMessage("Permarole prefix default", await _role.GetPermaPrefix(Context.Guild.Id)));
            }

            [Command("set")]
            public async Task PermaRolePrefixSet([Remainder] string message)
            {
                if (message.Equals(await _role.GetPermaPrefix(Context.Guild.Id), StringComparison.OrdinalIgnoreCase))
                {
                    await ReplyAsync(_localization.GetMessage("Invalid ar/pr prefix"));
                    return;
                }
                await _role.SetPermaPrefix(Context.Guild.Id, message);
                await ReplyAsync(_localization.GetMessage("Permarole prefix set", message));
            }
        }
    }
}