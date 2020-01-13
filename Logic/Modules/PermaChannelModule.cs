using Discord;
using Discord.Commands;
using IDal.Database;
using Logic.Services;
using System;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Alias("pc")]
    [Group("permachannel")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class PermaChannelModule : ModuleBase<SocketCommandContext>
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
        public class PermaChannelPrefixModule : ModuleBase<SocketCommandContext>
        {
            private IDbChannel _channel;
            private IDbLanguage _language;
            private LocalizationService _localization;

            public PermaChannelPrefixModule(IDbChannel channel, IDbLanguage language, LocalizationService localization)
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
            }

            [Command]
            public async Task DefaultPermaChannelPrefix()
            {
                await ReplyAsync(_localization.GetMessage("Permachannel prefix default", await _channel.GetPermaPrefix(Context.Guild.Id)));
            }

            [Command("set")]
            public async Task PermaChannelPrefixSet([Remainder] string message)
            {
                if (message.Equals(await _channel.GetPermaPrefix(Context.Guild.Id), StringComparison.OrdinalIgnoreCase))
                {
                    await ReplyAsync(_localization.GetMessage("Invalid ac/pc prefic"));
                    return;
                }
                await _channel.SetPermaPrefix(Context.Guild.Id, message);
                await ReplyAsync(_localization.GetMessage("Permachannel prefix set", message));
            }
        }

        [Group("name")]
        public class PermaChannelNameModule : ModuleBase<SocketCommandContext>
        {
            private IDbChannel _channel;
            private IDbLanguage _language;
            private LocalizationService _localization;

            public PermaChannelNameModule(IDbChannel channel, IDbLanguage language, LocalizationService localization)
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
            }

            [Command]
            public async Task DefaultPermaChannelName()
            {
                await ReplyAsync(_localization.GetMessage("Permachannel name default", await _channel.GetPermaName(Context.Guild.Id)));
            }

            [Command("set")]
            public async Task PermaChannelNameSet([Remainder] string message)
            {
                await _channel.SetPermaName(Context.Guild.Id, message);
                await ReplyAsync(_localization.GetMessage("Permachannel name set", message));
            }
        }
    }
}