namespace Logic.Modules
{
    using Core.Entity;
    using Discord;
    using Discord.Commands;
    using Exceptions;
    using IDal.Database;
    using Services;
    using System.Threading.Tasks;

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
            private readonly ChannelService _channel;
            private readonly IDbLanguage _language;
            private readonly LocalizationService _localization;

            private PermaChannel _data;

            public PermaChannelPrefixModule(ChannelService channel, IDbLanguage language, LocalizationService localization)
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
                _data = await _channel.LoadPerma(Context.Guild.Id);
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

                await ReplyAsync(_localization.GetMessage("Permachannel prefix set", message));
            }
        }

        [Group("name")]
        public class PermaChannelNameModule : ModuleBase<SocketCommandContext>
        {
            private readonly ChannelService _channel;
            private readonly IDbLanguage _language;
            private readonly LocalizationService _localization;

            private PermaChannel _data;

            public PermaChannelNameModule(ChannelService channel, IDbLanguage language, LocalizationService localization)
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
                _data = await _channel.LoadPerma(Context.Guild.Id);
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

                await ReplyAsync(_localization.GetMessage("Permachannel name set", message));
            }
        }
    }
}