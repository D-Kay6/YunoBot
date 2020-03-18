namespace Logic.Modules
{
    using Core.Entity;
    using System;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Exceptions;
    using IDal.Database;
    using Services;

    [Alias("pr")]
    [Group("permarole")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class PermaRoleModule : ModuleBase<SocketCommandContext>
    {
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;

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
            private readonly RoleService _role;
            private readonly IDbLanguage _language;
            private readonly LocalizationService _localization;

            private PermaRole _data;

            public PermaRolePrefixModule(RoleService role, IDbLanguage language, LocalizationService localization)
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
                _data = await _role.LoadPerma(Context.Guild.Id);
            }

            [Command]
            public async Task DefaultPermaRolePrefix()
            {
                await ReplyAsync(_localization.GetMessage("Permarole prefix default", _data.Prefix));
            }

            [Command("set")]
            public async Task PermaRolePrefixSet([Remainder] string message)
            {
                _data.Prefix = message;
                try
                {
                    await _role.Save(_data);
                }
                catch (InvalidPrefixException)
                {
                    await ReplyAsync(_localization.GetMessage("Permarole prefix invalid empty", message));
                    return;
                }
                catch (PrefixExistsException)
                {
                    await ReplyAsync(_localization.GetMessage("Permarole prefix invalid auto", message));
                    return;
                }

                await ReplyAsync(_localization.GetMessage("Permarole prefix set", message));
            }
        }
    }
}