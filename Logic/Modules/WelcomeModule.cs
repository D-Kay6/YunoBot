using Core.Entity;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using IDal.Database;
using Logic.Extensions;
using Logic.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Group("welcome")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class WelcomeModule : ModuleBase<SocketCommandContext>
    {
        private readonly IDbWelcome _welcome;
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;

        private WelcomeMessage _settings;

        public WelcomeModule(IDbWelcome welcome, IDbLanguage language, LocalizationService localization)
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
            _settings = await _welcome.GetWelcomeSettings(Context.Guild.Id);
        }

        [Priority(-1)]
        [Command]
        public async Task DefaultWelcome([Remainder] string name)
        {
            if (_settings == null)
            {
                await ReplyAsync(_localization.GetMessage("Welcome exception"));
                return;
            }

            if (string.IsNullOrWhiteSpace(name)) name = string.Empty;
            await ReplyAsync(_localization.GetMessage("Invalid user", name));
        }

        //[Priority(-1)]
        //[Command]
        //public async Task DefaultWelcome()
        //{
        //    if (_settings == null)
        //    {
        //        await ReplyAsync(_localization.GetMessage("Welcome exception"));
        //        return;
        //    }
        //    await ReplyAsync(_localization.GetMessage("Invalid user", ""));
        //}
        
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
            if (_settings == null)
            {
                await ReplyAsync(_localization.GetMessage("Welcome exception"));
                return;
            }
            var names = string.Join(", ", users.Select(u => u.Mention)).ReplaceLast(", ", " and ");
            var msg = string.Format(_settings.Message, names);

            if (!_settings.UseImage) await ReplyAsync(msg);
            else await Context.Channel.SendFileAsync(ImageExtensions.GetImagePath("GasaiYunoWelcome.jpg"), msg);
        }
        
        [Command("enable")]
        [Alias("on")]
        public async Task WelcomeEnable(SocketTextChannel channel)
        {
            await _welcome.Enable(Context.Guild.Id, channel.Id);
            await ReplyAsync(_localization.GetMessage("Welcome enable", channel.Mention));
        }
        
        [Command("disable")]
        [Alias("off")]
        public async Task WelcomeDisable()
        {
            await _welcome.Disable(Context.Guild.Id);
            await ReplyAsync(_localization.GetMessage("Welcome disable"));
        }

        [Group("message")]
        public class WelcomeMessageModule : ModuleBase<SocketCommandContext>
        {
            private IDbWelcome _welcome;
            private IDbLanguage _language;
            private LocalizationService _localization;
            private WelcomeMessage _settings;

            public WelcomeMessageModule(IDbWelcome welcome, IDbLanguage language, LocalizationService localization)
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
                _settings = await _welcome.GetWelcomeSettings(Context.Guild.Id);
            }

            [Command]
            public async Task DefaultWelcomeMessage()
            {
                if (_settings == null)
                {
                    await ReplyAsync(_localization.GetMessage("Welcome exception"));
                    return;
                }
                await ReplyAsync(_localization.GetMessage("Welcome message default", "{0}", _settings.Message, Context.Guild.Name));
            }

            [Command("set")]
            public async Task WelcomeMessageSet([Remainder] string message)
            {
                await _welcome.SetWelcomeMessage(Context.Guild.Id, message);
                await ReplyAsync(_localization.GetMessage("Welcome message set", message, Context.Guild.Name));
            }
        }

        [Group("image")]
        public class WelcomeImageModule : ModuleBase<SocketCommandContext>
        {
            private IDbWelcome _welcome;
            private IDbLanguage _language;
            private LocalizationService _localization;
            private WelcomeMessage _settings;

            public WelcomeImageModule(IDbWelcome welcome, IDbLanguage language, LocalizationService localization)
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
                _settings = await _welcome.GetWelcomeSettings(Context.Guild.Id);
            }

            [Command]
            public async Task DefaultWelcomeImage()
            {
                if (_settings == null)
                {
                    await ReplyAsync(_localization.GetMessage("Welcome exception"));
                    return;
                }
                await ReplyAsync(_localization.GetMessage("Welcome image default",
                    _localization.GetMessage(_settings.UseImage ? "Welcome image use" : "Welcome image not use")));
            }

            [Command("enable")]
            [Alias("on")]
            public async Task WelcomeImageEnable()
            {
                await _welcome.UseImage(Context.Guild.Id, true);
                await ReplyAsync(_localization.GetMessage("Welcome image enable", Context.Guild.Name));
            }

            [Command("disable")]
            [Alias("off")]
            public async Task WelcomeImageDisable()
            {
                await _welcome.UseImage(Context.Guild.Id, false);
                await ReplyAsync(_localization.GetMessage("Welcome image disable", Context.Guild.Name));
            }
        }
    }
}