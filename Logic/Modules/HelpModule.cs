using DalFactory;
using Discord;
using Discord.Commands;
using Logic.Extentions;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Group("help")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private Localization.Localization _lang;

        protected override void BeforeExecute(CommandInfo command)
        {
            _lang = new Localization.Localization(Context.Guild.Id);
            base.BeforeExecute(command);
        }

        [Priority(-1)]
        [Command]
        public async Task DefaultHelp([Remainder] string message = null)
        {
            var settings = DatabaseFactory.GenerateServerSettings();
            var prefix = settings.GetCommandPrefix(Context.Guild.Id);
            if (!string.IsNullOrEmpty(message))
            {
                await ReplyAsync(_lang.GetMessage("Help no info"));
                return;
            }
            var embed = new EmbedBuilder();
            embed.AddField("Help", _lang.GetMessage("Help title"));
            embed.AddField("__General Commands__", _lang.GetMessage("Help general", prefix));
            embed.AddField("Praise", _lang.GetMessage("Help praise", prefix));
            embed.AddField("music", _lang.GetMessage("Help music", prefix));
            embed.AddField("request", _lang.GetMessage("Help request", prefix));
            embed.AddField("__Admin Commands__", _lang.GetMessage("Help admin", prefix));
            embed.AddField("prefix", _lang.GetMessage("Help prefix", prefix));
            embed.AddField("language", _lang.GetMessage("Help language", prefix));
            embed.AddField("welcome", _lang.GetMessage("Help welcome", prefix));
            embed.AddField("autochannel (ac)", _lang.GetMessage("Help autochannel", prefix));
            embed.AddField("permachannel (pc)", _lang.GetMessage("Help permachannel", prefix));
            embed.AddField("autorole (ar)", _lang.GetMessage("Help autorole", prefix));
            embed.AddField("permarole (pr)", _lang.GetMessage("Help permachannel", prefix));
            await ReplyAsync("", false, embed.Build());
        }

        [Command("praise")]
        public async Task HelpPraise()
        {
            var settings = DatabaseFactory.GenerateServerSettings();
            var prefix = settings.GetCommandPrefix(Context.Guild.Id);
            var embed = EmbedExtentions.CreateEmbed("Help praise", _lang.GetMessage("Help praise title") + "\n\n" + _lang.GetMessage("Help praise", prefix));
            await ReplyAsync("", false, embed);
        }

        [Command("music")]
        public async Task HelpMusic()
        {
            var settings = DatabaseFactory.GenerateServerSettings();
            var prefix = settings.GetCommandPrefix(Context.Guild.Id);
            var embed = EmbedExtentions.CreateEmbed("Help music", _lang.GetMessage("Help music title") + "\n\n" + _lang.GetMessage("Help music", prefix));
            await ReplyAsync("", false, embed);
        }

        [Command("request")]
        public async Task HelpRequest()
        {
            var settings = DatabaseFactory.GenerateServerSettings();
            var prefix = settings.GetCommandPrefix(Context.Guild.Id);
            var embed = EmbedExtentions.CreateEmbed("Help request", _lang.GetMessage("Help request title") + "\n\n" + _lang.GetMessage("Help request", prefix));
            await ReplyAsync("", false, embed);
        }

        [Command("prefix")]
        public async Task HelpPrefix()
        {
            var settings = DatabaseFactory.GenerateServerSettings();
            var prefix = settings.GetCommandPrefix(Context.Guild.Id);
            var embed = EmbedExtentions.CreateEmbed("Help prefix", _lang.GetMessage("Help prefix title") + "\n\n" + _lang.GetMessage("Help prefix", prefix));
            await ReplyAsync("", false, embed);
        }

        [Command("language")]
        public async Task HelpLanguage()
        {
            var settings = DatabaseFactory.GenerateServerSettings();
            var prefix = settings.GetCommandPrefix(Context.Guild.Id);
            var embed = EmbedExtentions.CreateEmbed("Help language", _lang.GetMessage("Help language title") + "\n\n" + _lang.GetMessage("Help language", prefix));
            await ReplyAsync("", false, embed);
        }

        [Command("welcome")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task HelpWelcome()
        {
            var settings = DatabaseFactory.GenerateServerSettings();
            var prefix = settings.GetCommandPrefix(Context.Guild.Id);
            var embed = EmbedExtentions.CreateEmbed("Help welcome", _lang.GetMessage("Help welcome title") + "\n\n" + _lang.GetMessage("Help welcome", prefix));
            await ReplyAsync("", false, embed);
        }

        [Alias("ac")]
        [Command("autochannel")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task HelpAutoChannel()
        {
            var settings = DatabaseFactory.GenerateServerSettings();
            var prefix = settings.GetCommandPrefix(Context.Guild.Id);
            var embed = EmbedExtentions.CreateEmbed("Help autochannel", _lang.GetMessage("Help autochannel title") + "\n\n" + _lang.GetMessage("Help autochannel", prefix));
            await ReplyAsync("", false, embed);
        }

        [Alias("pc")]
        [Command("permachannel")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task HelpPermaChannel()
        {
            var settings = DatabaseFactory.GenerateServerSettings();
            var prefix = settings.GetCommandPrefix(Context.Guild.Id);
            var embed = EmbedExtentions.CreateEmbed("Help permachannel", _lang.GetMessage("Help permachannel title") + "\n\n" + _lang.GetMessage("Help permachannel", prefix));
            await ReplyAsync("", false, embed);
        }

        [Alias("ar")]
        [Command("autorole")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task HelpAutoRole()
        {
            var settings = DatabaseFactory.GenerateServerSettings();
            var prefix = settings.GetCommandPrefix(Context.Guild.Id);
            var embed = EmbedExtentions.CreateEmbed("Help autorole", _lang.GetMessage("Help autorole title") + "\n\n" + _lang.GetMessage("Help autorole", prefix));
            await ReplyAsync("", false, embed);
        }

        [Alias("pr")]
        [Command("permarole")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task HelpPermaRole()
        {
            var settings = DatabaseFactory.GenerateServerSettings();
            var prefix = settings.GetCommandPrefix(Context.Guild.Id);
            var embed = EmbedExtentions.CreateEmbed("Help permarole", _lang.GetMessage("Help permarole title") + "\n\n" + _lang.GetMessage("Help permarole", prefix));
            await ReplyAsync("", false, embed);
        }
    }
}