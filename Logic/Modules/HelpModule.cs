using System;
using DalFactory;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using IDal.Interfaces.Database;
using Logic.Extensions;

namespace Logic.Modules
{
    [Group("help")]
    public class HelpModule : ModuleBase<SocketCommandContext>
    {
        private IDbCommand _command;
        private IDbLanguage _language;

        private string _prefix;
        private Localization.Localization _lang;

        public HelpModule(IDbCommand command, IDbLanguage language)
        {
            _command = command;
            _language = language;
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            Task.WaitAll(LoadPrefix(), LoadLanguage());
            base.BeforeExecute(command);
        }

        private async Task LoadPrefix()
        {
            _prefix = await _command.GetPrefix(Context.Guild.Id);
        }

        private async Task LoadLanguage()
        {
            _lang = new Localization.Localization(await _language.GetLanguage(Context.Guild.Id));
        }

        [Priority(-1)]
        [Command]
        public async Task DefaultHelp([Remainder] string message = null)
        {
            if (!string.IsNullOrEmpty(message))
            {
                await ReplyAsync(_lang.GetMessage("Help no info"));
                return;
            }

            try
            {

                var embed = new EmbedBuilder();
                embed.AddField("Help", _lang.GetMessage("Help title"));
                embed.AddField("__General Commands__", _lang.GetMessage("Help general", _prefix));
                embed.AddField("Praise", _lang.GetMessage("Help praise", _prefix));
                embed.AddField("music", _lang.GetMessage("Help music", _prefix));
                embed.AddField("request", _lang.GetMessage("Help request", _prefix));
                embed.AddField("command", _lang.GetMessage("Help command", _prefix));
                embed.AddField("__Admin Commands__", _lang.GetMessage("Help admin", _prefix));
                embed.AddField("command", _lang.GetMessage("Help command admin", _prefix));
                embed.AddField("language", _lang.GetMessage("Help language", _prefix));
                embed.AddField("welcome", _lang.GetMessage("Help welcome", _prefix));
                embed.AddField("autochannel (ac)", _lang.GetMessage("Help autochannel", _prefix));
                embed.AddField("permachannel (pc)", _lang.GetMessage("Help permachannel", _prefix));
                embed.AddField("autorole (ar)", _lang.GetMessage("Help autorole", _prefix));
                embed.AddField("permarole (pr)", _lang.GetMessage("Help permarole", _prefix));
                embed.AddField("kick", _lang.GetMessage("Help kick", _prefix));
                embed.AddField("ban", _lang.GetMessage("Help ban", _prefix));
                await ReplyAsync("", false, embed.Build());
            }
            catch (Exception e)
            {

            }
        }

        [Command("praise")]
        public async Task HelpPraise()
        {
            var embed = EmbedExtensions.CreateEmbed("Help praise", _lang.GetMessage("Help praise title") + "\n\n" + _lang.GetMessage("Help praise", _prefix));
            await ReplyAsync("", false, embed);
        }

        [Command("music")]
        public async Task HelpMusic()
        {
            var embed = EmbedExtensions.CreateEmbed("Help music", _lang.GetMessage("Help music title") + "\n\n" + _lang.GetMessage("Help music", _prefix));
            await ReplyAsync("", false, embed);
        }

        [Command("request")]
        public async Task HelpRequest()
        {
            var embed = EmbedExtensions.CreateEmbed("Help request", _lang.GetMessage("Help request title") + "\n\n" + _lang.GetMessage("Help request", _prefix));
            await ReplyAsync("", false, embed);
        }

        [Command("command")]
        public async Task HelpCommand()
        {
            var embed = EmbedExtensions.CreateEmbed("Help command", _lang.GetMessage("Help command title") + "\n\n" + _lang.GetMessage("Help command", _prefix) + "\n" + _lang.GetMessage("Help command admin", _prefix));
            await ReplyAsync("", false, embed);
        }

        [Command("language")]
        public async Task HelpLanguage()
        {
            var embed = EmbedExtensions.CreateEmbed("Help language", _lang.GetMessage("Help language title") + "\n\n" + _lang.GetMessage("Help language", _prefix));
            await ReplyAsync("", false, embed);
        }

        [Command("welcome")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task HelpWelcome()
        {
            var embed = EmbedExtensions.CreateEmbed("Help welcome", _lang.GetMessage("Help welcome title") + "\n\n" + _lang.GetMessage("Help welcome", _prefix));
            await ReplyAsync("", false, embed);
        }

        [Alias("ac")]
        [Command("autochannel")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task HelpAutoChannel()
        {
            var embed = EmbedExtensions.CreateEmbed("Help autochannel", _lang.GetMessage("Help autochannel title") + "\n\n" + _lang.GetMessage("Help autochannel", _prefix));
            await ReplyAsync("", false, embed);
        }

        [Alias("pc")]
        [Command("permachannel")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task HelpPermaChannel()
        {
            var embed = EmbedExtensions.CreateEmbed("Help permachannel", _lang.GetMessage("Help permachannel title") + "\n\n" + _lang.GetMessage("Help permachannel", _prefix));
            await ReplyAsync("", false, embed);
        }

        [Alias("ar")]
        [Command("autorole")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task HelpAutoRole()
        {
            var embed = EmbedExtensions.CreateEmbed("Help autorole", _lang.GetMessage("Help autorole title") + "\n\n" + _lang.GetMessage("Help autorole", _prefix));
            await ReplyAsync("", false, embed);
        }

        [Alias("pr")]
        [Command("permarole")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task HelpPermaRole()
        {
            var embed = EmbedExtensions.CreateEmbed("Help permarole", _lang.GetMessage("Help permarole title") + "\n\n" + _lang.GetMessage("Help permarole", _prefix));
            await ReplyAsync("", false, embed);
        }
    }
}