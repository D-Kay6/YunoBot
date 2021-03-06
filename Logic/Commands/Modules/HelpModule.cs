﻿using Discord;
using Discord.Commands;
using IDal.Database;
using Logic.Extensions;
using Logic.Services;
using System;
using System.Threading.Tasks;
using CommandService = Logic.Services.CommandService;

namespace Logic.Commands.Modules
{
    [Group("help")]
    public class HelpModule : ModuleBase<ShardedCommandContext>
    {
        private readonly CommandService _command;
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;

        private string _prefix;

        public HelpModule(CommandService command, IDbLanguage language, LocalizationService localization)
        {
            _command = command;
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
            _prefix = await _command.GetPrefix(Context.Guild.Id);
            await _localization.Load(await _language.GetLanguage(Context.Guild.Id));
        }

        [Priority(-1)]
        [Command]
        public async Task DefaultHelp([Remainder] string message = null)
        {
            if (!string.IsNullOrEmpty(message))
            {
                await ReplyAsync(_localization.GetMessage("Help no info"));
                return;
            }

            try
            {
                var embed = new EmbedBuilder();
                embed.AddField("Help", _localization.GetMessage("Help title"));
                embed.AddField("__General Commands__", _localization.GetMessage("Help general", _prefix));
                embed.AddField("Praise", _localization.GetMessage("Help praise", _prefix));
                embed.AddField("Music", _localization.GetMessage("Help music", _prefix));
                embed.AddField("Request", _localization.GetMessage("Help request", _prefix));
                embed.AddField("Command", _localization.GetMessage("Help command", _prefix));
                embed.AddField("__Admin Commands__", _localization.GetMessage("Help admin", _prefix));
                embed.AddField("Command", _localization.GetMessage("Help command admin", _prefix));
                embed.AddField("Language", _localization.GetMessage("Help language", _prefix));
                embed.AddField("Welcome", _localization.GetMessage("Help welcome", _prefix));
                embed.AddField("AutoChannel (ac)", _localization.GetMessage("Help autochannel", _prefix));
                embed.AddField("PermaChannel (pc)", _localization.GetMessage("Help permachannel", _prefix));
                embed.AddField("DynamicRole (dr)", _localization.GetMessage("Help dynamicrole", _prefix));
                embed.AddField("Kick", _localization.GetMessage("Help kick", _prefix));
                embed.AddField("Ban", _localization.GetMessage("Help ban", _prefix));
                await ReplyAsync("", false, embed.Build());
            }
            catch (Exception)
            {
            }
        }

        [Command("praise")]
        public async Task HelpPraise()
        {
            var embed = EmbedExtensions.CreateEmbed("Help praise",
                _localization.GetMessage("Help praise title") + "\n\n" +
                _localization.GetMessage("Help praise", _prefix));
            await ReplyAsync("", false, embed);
        }

        [Command("music")]
        public async Task HelpMusic()
        {
            var embed = EmbedExtensions.CreateEmbed("Help music",
                _localization.GetMessage("Help music title") + "\n\n" +
                _localization.GetMessage("Help music", _prefix));
            await ReplyAsync("", false, embed);
        }

        [Command("request")]
        public async Task HelpRequest()
        {
            var embed = EmbedExtensions.CreateEmbed("Help request",
                _localization.GetMessage("Help request title") + "\n\n" +
                _localization.GetMessage("Help request", _prefix));
            await ReplyAsync("", false, embed);
        }

        [Command("command")]
        public async Task HelpCommand()
        {
            var embed = EmbedExtensions.CreateEmbed("Help command",
                _localization.GetMessage("Help command title") + "\n\n" +
                _localization.GetMessage("Help command", _prefix) + "\n" +
                _localization.GetMessage("Help command admin", _prefix));
            await ReplyAsync("", false, embed);
        }

        [Command("language")]
        public async Task HelpLanguage()
        {
            var embed = EmbedExtensions.CreateEmbed("Help language",
                _localization.GetMessage("Help language title") + "\n\n" +
                _localization.GetMessage("Help language", _prefix));
            await ReplyAsync("", false, embed);
        }

        [Command("welcome")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task HelpWelcome()
        {
            var embed = EmbedExtensions.CreateEmbed("Help welcome",
                _localization.GetMessage("Help welcome title") + "\n\n" +
                _localization.GetMessage("Help welcome", _prefix));
            await ReplyAsync("", false, embed);
        }

        [Alias("ac")]
        [Command("autochannel")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task HelpAutoChannel()
        {
            var embed = EmbedExtensions.CreateEmbed("Help autochannel",
                _localization.GetMessage("Help autochannel title") + "\n\n" +
                _localization.GetMessage("Help autochannel", _prefix));
            await ReplyAsync("", false, embed);
        }

        [Alias("pc")]
        [Command("permachannel")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task HelpPermaChannel()
        {
            var embed = EmbedExtensions.CreateEmbed("Help permachannel",
                _localization.GetMessage("Help permachannel title") + "\n\n" +
                _localization.GetMessage("Help permachannel", _prefix));
            await ReplyAsync("", false, embed);
        }

        [Alias("dr")]
        [Command("dynamicrole")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task HelpAutoRole()
        {
            var embed = EmbedExtensions.CreateEmbed("Help dynamicrole",
                _localization.GetMessage("Help dynamicrole title") + "\n\n" +
                _localization.GetMessage("Help dynamicrole", _prefix));
            await ReplyAsync("", false, embed);
        }
    }
}