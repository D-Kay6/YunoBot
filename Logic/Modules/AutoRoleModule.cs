namespace Logic.Modules
{
    using Core.Entity;
    using Discord;
    using Discord.Commands;
    using Exceptions;
    using IDal.Database;
    using Services;
    using System.Threading.Tasks;

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
            private readonly RoleService _role;
            private readonly IDbLanguage _language;
            private readonly LocalizationService _localization;

            private AutoRole _data;

            public AutoRolePrefixModule(RoleService role, IDbLanguage language, LocalizationService localization)
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
                _data = await _role.LoadAuto(Context.Guild.Id);
            }

            [Command]
            public async Task DefaultAutoRolePrefix()
            {
                await ReplyAsync(_localization.GetMessage("Autorole prefix default", _data.Prefix));
            }

            [Command("set")]
            public async Task AutoRolePrefixSet([Remainder] string message)
            {
                _data.Prefix = message;
                try
                {
                    await _role.Save(_data);
                }
                catch (InvalidPrefixException)
                {
                    await ReplyAsync(_localization.GetMessage("Autorole prefix invalid empty", message));
                    return;
                }
                catch (PrefixExistsException)
                {
                    await ReplyAsync(_localization.GetMessage("Autorole prefix invalid perma", message));
                    return;
                }

                await ReplyAsync(_localization.GetMessage("Autorole prefix set", message));
            }
        }

        [Group("ignore")]
        public class AutoRoleIgnoreModule : ModuleBase<SocketCommandContext>
        {
            private readonly RoleService _role;
            private readonly IDbLanguage _language;
            private readonly LocalizationService _localization;
            
            public AutoRoleIgnoreModule(RoleService role, IDbLanguage language, LocalizationService localization)
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