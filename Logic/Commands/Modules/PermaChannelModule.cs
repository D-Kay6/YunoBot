using Core.Entity;
using Core.Enum;
using Discord;
using Discord.Commands;
using IDal.Database;
using Logic.Exceptions;
using Logic.Services;
using System;
using System.Threading.Tasks;

namespace Logic.Commands.Modules
{
    [Alias("pc")]
    [Group("permachannel")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class PermaChannelModule : ModuleBase<ShardedCommandContext>
    {
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;

        public PermaChannelModule(IDbLanguage language, LocalizationService localization)
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
        public async Task DefaultPermaChannel()
        {
            await ReplyAsync(_localization.GetMessage("Permachannel default"));
        }

        [Group("prefix")]
        public class PermaChannelPrefixModule : ModuleBase<ShardedCommandContext>
        {
            private readonly DynamicChannelService _channel;
            private readonly LogsService _logs;
            private readonly IDbLanguage _language;
            private readonly LocalizationService _localization;

            private DynamicChannel _data;

            public PermaChannelPrefixModule(DynamicChannelService channel, LogsService logs, IDbLanguage language, LocalizationService localization)
            {
                _channel = channel;
                _logs = logs;
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
                _data = await _channel.Load(Context.Guild.Id, AutomationType.Permanent);
            }

            [Command]
            public async Task DefaultPermaChannelPrefix()
            {
                await ReplyAsync(_localization.GetMessage("Permachannel prefix default", _data.Prefix));
            }

            [Command("set")]
            public async Task PermaChannelPrefixSet([Remainder] string message)
            {
                _data.Prefix = message;
                try
                {
                    await _channel.Save(_data);
                }
                catch (InvalidPrefixException)
                {
                    await ReplyAsync(_localization.GetMessage("Permachannel prefix invalid empty", message));
                    return;
                }
                catch (PrefixExistsException)
                {
                    await ReplyAsync(_localization.GetMessage("Permachannel prefix invalid auto", message));
                    return;
                }
                catch (Exception e)
                {
                    await ReplyAsync(_localization.GetMessage("General error"));
                    await _logs.Write("Errors", "Changing an permachannel name broke.", e, Context.Guild);
                }

                await ReplyAsync(_localization.GetMessage("Permachannel prefix set", message));
            }
        }

        [Group("name")]
        public class PermaChannelNameModule : ModuleBase<ShardedCommandContext>
        {
            private readonly DynamicChannelService _channel;
            private readonly LogsService _logs;
            private readonly IDbLanguage _language;
            private readonly LocalizationService _localization;

            private DynamicChannel _data;

            public PermaChannelNameModule(DynamicChannelService channel, LogsService logs, IDbLanguage language, LocalizationService localization)
            {
                _channel = channel;
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
                _data = await _channel.Load(Context.Guild.Id, AutomationType.Permanent);
            }

            [Command]
            public async Task DefaultPermaChannelName()
            {
                await ReplyAsync(_localization.GetMessage("Permachannel name default", _data.Name));
            }

            [Command("set")]
            public async Task PermaChannelNameSet([Remainder] string message)
            {
                _data.Name = message;
                try
                {
                    await _channel.Save(_data);
                }
                catch (InvalidNameException)
                {
                    await ReplyAsync(_localization.GetMessage("Permachannel name invalid empty", message));
                }
                catch (Exception e)
                {
                    await ReplyAsync(_localization.GetMessage("General error"));
                    await _logs.Write("Errors", $"Changing an permachannel name broke.", e, Context.Guild);
                }

                await ReplyAsync(_localization.GetMessage("Permachannel name set", message));
            }
        }
    }
}