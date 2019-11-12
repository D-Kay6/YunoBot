using DalFactory;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;
using IDal.Interfaces.Database;
using IDal.Structs.Database;
using Logic.Extensions;
using Logic.Services;

namespace Logic.Modules
{
    [Group("welcome")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class WelcomeModule : ModuleBase<SocketCommandContext>
    {
        private Localization.Localization _lang;
        private IWelcomeMessage _connection;
        private WelcomeData _settings;

        protected override void BeforeExecute(CommandInfo command)
        {
            _lang = new Localization.Localization(Context.Guild.Id);
            _connection = DatabaseFactory.GenerateWelcomeMessage();
            _settings = _connection.GetWelcomeMessage(Context.Guild.Id);
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
            var welcome = DatabaseFactory.GenerateWelcomeMessage();
            var settings = welcome.GetWelcomeMessage(Context.Guild.Id);
            if (settings == null)
            {
                await ReplyAsync(_lang.GetMessage("Welcome exception"));
                return;
            }
            var names = string.Join(", ", users.Select(u => u.Mention)).ReplaceLast(", ", " and ");
            var msg = string.Format(settings.Message, names);

            if (!settings.UseImage) await ReplyAsync(msg);
            else await Context.Channel.SendFileAsync(ImageExtensions.GetImagePath("GasaiYunoWelcome.jpg"), msg);
        }
        
        [Command("enable")]
        [Alias("on")]
        public async Task WelcomeEnable(SocketTextChannel channel)
        {
            var welcome = DatabaseFactory.GenerateWelcomeMessage();
            welcome.Enable(Context.Guild.Id, channel.Id);
            await ReplyAsync(_lang.GetMessage("Welcome enable", channel.Mention));
        }
        
        [Command("disable")]
        [Alias("off")]
        public async Task WelcomeDisable()
        {
            var welcome = DatabaseFactory.GenerateWelcomeMessage();
            welcome.Disable(Context.Guild.Id);
            await ReplyAsync(_lang.GetMessage("Welcome disable"));
        }

        [Group("message")]
        public class WelcomeMessageModule : ModuleBase<SocketCommandContext>
        {
            private Localization.Localization _lang;

            protected override void BeforeExecute(CommandInfo command)
            {
                _lang = new Localization.Localization(Context.Guild.Id);
                base.BeforeExecute(command);
            }

            [Command]
            public async Task DefaultWelcomeMessage()
            {
                var welcome = DatabaseFactory.GenerateWelcomeMessage();
                var settings = welcome.GetWelcomeMessage(Context.Guild.Id);
                if (settings == null)
                {
                    await ReplyAsync(_lang.GetMessage("Welcome exception"));
                    return;
                }
                await ReplyAsync(_lang.GetMessage("Welcome message default", "{0}", settings.Message, Context.Guild.Name));
            }

            [Command("set")]
            public async Task WelcomeMessageSet([Remainder] string message)
            {
                var welcome = DatabaseFactory.GenerateWelcomeMessage();
                welcome.SetWelcomeMessage(Context.Guild.Id, message);
                await ReplyAsync(_lang.GetMessage("Welcome message set", message, Context.Guild.Name));
            }
        }

        [Group("image")]
        public class WelcomeImageModule : ModuleBase<SocketCommandContext>
        {
            private Localization.Localization _lang;

            protected override void BeforeExecute(CommandInfo command)
            {
                _lang = new Localization.Localization(Context.Guild.Id);
                base.BeforeExecute(command);
            }

            [Command]
            public async Task DefaultWelcomeImage()
            {
                var welcome = DatabaseFactory.GenerateWelcomeMessage();
                var settings = welcome.GetWelcomeMessage(Context.Guild.Id);
                if (settings == null)
                {
                    await ReplyAsync(_lang.GetMessage("Welcome exception"));
                    return;
                }
                await ReplyAsync(_lang.GetMessage("Welcome image default",
                    _lang.GetMessage(settings.UseImage ? "Welcome image use" : "Welcome image not use")));
            }

            [Command("enable")]
            [Alias("on")]
            public async Task WelcomeImageEnable()
            {
                var welcome = DatabaseFactory.GenerateWelcomeMessage();
                welcome.UseImage(Context.Guild.Id, true);
                await ReplyAsync(_lang.GetMessage("Welcome image enable", Context.Guild.Name));
            }

            [Command("disable")]
            [Alias("off")]
            public async Task WelcomeImageDisable()
            {
                var welcome = DatabaseFactory.GenerateWelcomeMessage();
                welcome.UseImage(Context.Guild.Id, false);
                await ReplyAsync(_lang.GetMessage("Welcome image disable", Context.Guild.Name));
            }
        }
    }
}
