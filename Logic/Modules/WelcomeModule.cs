using Logic.Exceptions;

namespace Logic.Modules
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Entity;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using Extensions;
    using IDal.Database;
    using Services;

    [Group("welcome")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class WelcomeModule : ModuleBase<SocketCommandContext>
    {
        private readonly WelcomeService _welcome;
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;

        public WelcomeModule(WelcomeService welcome, IDbLanguage language, LocalizationService localization)
        {
            _welcome = welcome;
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

        [Priority(-1)]
        [Command]
        public async Task DefaultWelcome([Remainder] string name)
        {
            if (string.IsNullOrWhiteSpace(name)) name = string.Empty;
            await ReplyAsync(_localization.GetMessage("Invalid user", name));
        }

        [Command]
        public async Task DefaultWelcome(params SocketGuildUser[] users)
        {
            if (!users.Any())
            {
                var index = Context.Message.Content.IndexOf("welcome", StringComparison.OrdinalIgnoreCase);
                var param = Context.Message.Content.Substring(index + 7);
                await DefaultWelcome(param);
                return;
            }

            await _welcome.Welcome(Context.Channel as ITextChannel, users);
        }

        [Command("enable")]
        [Alias("on")]
        public async Task WelcomeEnable(SocketTextChannel channel)
        {
            var settings = await _welcome.Load(Context.Guild.Id);
            settings.ChannelId = channel.Id;
            await _welcome.Save(settings);
            await ReplyAsync(_localization.GetMessage("Welcome enable", channel.Mention));
        }

        [Command("disable")]
        [Alias("off")]
        public async Task WelcomeDisable()
        {
            var settings = await _welcome.Load(Context.Guild.Id);
            settings.ChannelId = null;
            await _welcome.Save(settings);
            await ReplyAsync(_localization.GetMessage("Welcome disable"));
        }

        [Group("message")]
        public class WelcomeMessageModule : ModuleBase<SocketCommandContext>
        {
            private readonly WelcomeService _welcome;
            private readonly IDbLanguage _language;
            private readonly LocalizationService _localization;

            private WelcomeMessage _settings;

            public WelcomeMessageModule(WelcomeService welcome, IDbLanguage language, LocalizationService localization)
            {
                _welcome = welcome;
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
                _settings = await _welcome.Load(Context.Guild.Id);
            }

            [Command]
            public async Task DefaultWelcomeMessage()
            {
                if (_settings == null)
                {
                    await ReplyAsync(_localization.GetMessage("Welcome exception"));
                    return;
                }

                await ReplyAsync(_localization.GetMessage("Welcome message default", "{0}", _settings.Message,
                    Context.Guild.Name));
            }

            [Command("set")]
            public async Task WelcomeMessageSet([Remainder] string message)
            {
                _settings.Message = message;
                try
                {
                    await _welcome.Save(_settings);
                }
                catch (InvalidMessageException)
                {
                    await ReplyAsync(_localization.GetMessage("Welcome message invalid"));
                    return;
                }

                await ReplyAsync(_localization.GetMessage("Welcome message set", message, Context.Guild.Name));
            }
        }

        [Group("image")]
        public class WelcomeImageModule : ModuleBase<SocketCommandContext>
        {
            private readonly WelcomeService _welcome;
            private readonly IDbLanguage _language;
            private readonly LocalizationService _localization;

            private WelcomeMessage _settings;

            public WelcomeImageModule(WelcomeService welcome, IDbLanguage language, LocalizationService localization)
            {
                _welcome = welcome;
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
                _settings = await _welcome.Load(Context.Guild.Id);
            }

            [Command]
            public async Task DefaultWelcomeImage()
            {
                await ReplyAsync(_localization.GetMessage("Welcome image default",
                    _localization.GetMessage(_settings.UseImage ? "Welcome image use" : "Welcome image not use")));
            }

            [Command("enable")]
            [Alias("on")]
            public async Task WelcomeImageEnable()
            {
                _settings.UseImage = true;
                await _welcome.Save(_settings);
                await ReplyAsync(_localization.GetMessage("Welcome image enable", Context.Guild.Name));
            }

            [Command("disable")]
            [Alias("off")]
            public async Task WelcomeImageDisable()
            {
                _settings.UseImage = false;
                await _welcome.Save(_settings);
                await ReplyAsync(_localization.GetMessage("Welcome image disable", Context.Guild.Name));
            }
        }
    }
}