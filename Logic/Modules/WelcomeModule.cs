using DalFactory;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Entity;
using IDal.Interfaces.Database;
using Logic.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Group("welcome")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class WelcomeModule : ModuleBase<SocketCommandContext>
    {
        private IWelcome _welcome;
        private Localization.Localization _lang;
        private WelcomeMessage _settings;

        public WelcomeModule(IWelcome welcome)
        {
            _welcome = welcome;
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            _lang = new Localization.Localization(Context.Guild.Id);
            _settings = _welcome.GetWelcomeSettings(Context.Guild.Id);
            base.BeforeExecute(command);
        }

        [Priority(-1)]
        [Command]
        public async Task DefaultWelcome([Remainder] string name)
        {
            await ReplyAsync(_lang.GetMessage("Unknown user", name));
        }

        [Priority(-1)]
        [Command]
        public async Task DefaultWelcome()
        {
            if (_settings == null)
            {
                await ReplyAsync(_lang.GetMessage("Welcome exception"));
                return;
            }
            await ReplyAsync(_lang.GetMessage("Unknown user", ""));
        }
        
        [Command]
        public async Task DefaultWelcome(params SocketGuildUser[] users)
        {
            if (_settings == null)
            {
                await ReplyAsync(_lang.GetMessage("Welcome exception"));
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
            _welcome.Enable(Context.Guild.Id, channel.Id);
            await ReplyAsync(_lang.GetMessage("Welcome enable", channel.Mention));
        }
        
        [Command("disable")]
        [Alias("off")]
        public async Task WelcomeDisable()
        {
            _welcome.Disable(Context.Guild.Id);
            await ReplyAsync(_lang.GetMessage("Welcome disable"));
        }

        [Group("message")]
        public class WelcomeMessageModule : ModuleBase<SocketCommandContext>
        {
            private IWelcome _welcome;
            private Localization.Localization _lang;
            private WelcomeMessage _settings;

            public WelcomeMessageModule(IWelcome welcome)
            {
                _welcome = welcome;
            }

            protected override void BeforeExecute(CommandInfo command)
            {
                _lang = new Localization.Localization(Context.Guild.Id);
                _settings = _welcome.GetWelcomeSettings(Context.Guild.Id);
                base.BeforeExecute(command);
            }

            [Command]
            public async Task DefaultWelcomeMessage()
            {
                if (_settings == null)
                {
                    await ReplyAsync(_lang.GetMessage("Welcome exception"));
                    return;
                }
                await ReplyAsync(_lang.GetMessage("Welcome message default", "{0}", _settings.Message, Context.Guild.Name));
            }

            [Command("set")]
            public async Task WelcomeMessageSet([Remainder] string message)
            {
                _welcome.SetWelcomeMessage(Context.Guild.Id, message);
                await ReplyAsync(_lang.GetMessage("Welcome message set", message, Context.Guild.Name));
            }
        }

        [Group("image")]
        public class WelcomeImageModule : ModuleBase<SocketCommandContext>
        {
            private IWelcome _welcome;
            private Localization.Localization _lang;
            private WelcomeMessage _settings;

            public WelcomeImageModule(IWelcome welcome)
            {
                _welcome = welcome;
            }

            protected override void BeforeExecute(CommandInfo command)
            {
                _lang = new Localization.Localization(Context.Guild.Id);
                _settings = _welcome.GetWelcomeSettings(Context.Guild.Id);
                base.BeforeExecute(command);
            }

            [Command]
            public async Task DefaultWelcomeImage()
            {
                if (_settings == null)
                {
                    await ReplyAsync(_lang.GetMessage("Welcome exception"));
                    return;
                }
                await ReplyAsync(_lang.GetMessage("Welcome image default",
                    _lang.GetMessage(_settings.UseImage ? "Welcome image use" : "Welcome image not use")));
            }

            [Command("enable")]
            [Alias("on")]
            public async Task WelcomeImageEnable()
            {
                _welcome.UseImage(Context.Guild.Id, true);
                await ReplyAsync(_lang.GetMessage("Welcome image enable", Context.Guild.Name));
            }

            [Command("disable")]
            [Alias("off")]
            public async Task WelcomeImageDisable()
            {
                _welcome.UseImage(Context.Guild.Id, false);
                await ReplyAsync(_lang.GetMessage("Welcome image disable", Context.Guild.Name));
            }
        }
    }
}
