namespace Logic.Modules
{
    using Core.Entity;
    using Discord;
    using Discord.Commands;
    using Exceptions;
    using IDal.Database;
    using Services;
    using System.Linq;
    using System.Threading.Tasks;

    [Alias("ac")]
    [Group("autochannel")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class AutoChannelModule : ModuleBase<SocketCommandContext>
    {
        private readonly ChannelService _channel;
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;

        public AutoChannelModule(IDbLanguage language, ChannelService channel, LocalizationService localization)
        {
            _language = language;
            _channel = channel;
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
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DefaultAutoChannel()
        {
            await ReplyAsync(_localization.GetMessage("Autochannel default"));
        }

        [Command("delete")]
        public async Task AutoChannelDelete()
        {
            var data = await _channel.LoadAuto(Context.Guild.Id);
            var channels = Context.Guild.VoiceChannels.Where(c => c.Name.StartsWith(data.Name));
            foreach (var channel in channels)
            {
                try
                {
                    await _channel.RemoveGeneratedChannel(Context.Guild.Id, channel.Id);
                }
                catch (InvalidChannelException)
                {
                    //TODO: Add logging
                }

                await channel.DeleteAsync();
            }

            await ReplyAsync(_localization.GetMessage("Autochannel delete", data.Name));
        }

        [Group("prefix")]
        public class AutoChannelPrefixModule : ModuleBase<SocketCommandContext>
        {
            private readonly ChannelService _channel;
            private readonly IDbLanguage _language;
            private readonly LocalizationService _localization;

            private AutoChannel _data;

            public AutoChannelPrefixModule(IDbLanguage language, ChannelService channel, LocalizationService localization)
            {
                _language = language;
                _channel = channel;
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
                _data = await _channel.LoadAuto(Context.Guild.Id);
            }

            [Command]
            public async Task DefaultAutoChannelPrefix()
            {
                await ReplyAsync(_localization.GetMessage("Autochannel prefix default", _data.Prefix));
            }

            [Command("set")]
            public async Task AutoChannelPrefixSet([Remainder] string message)
            {
                _data.Prefix = message;
                try
                {
                    await _channel.Save(_data);
                }
                catch (InvalidPrefixException)
                {
                    await ReplyAsync(_localization.GetMessage("Autochannel prefix invalid empty"));
                    return;
                }
                catch (PrefixExistsException)
                {
                    await ReplyAsync(_localization.GetMessage("Autochannel prefix invalid perma"));
                    return;
                }

                await ReplyAsync(_localization.GetMessage("Autochannel prefix set", message));
            }
        }

        [Group("name")]
        public class AutoChannelNameModule : ModuleBase<SocketCommandContext>
        {
            private readonly ChannelService _channel;
            private readonly IDbLanguage _language;
            private readonly LocalizationService _localization;

            private AutoChannel _data;

            public AutoChannelNameModule(IDbLanguage language, ChannelService channel, LocalizationService localization)
            {
                _language = language;
                _channel = channel;
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
                _data = await _channel.LoadAuto(Context.Guild.Id);
            }

            [Command]
            public async Task DefaultAutoChannelName()
            {
                await ReplyAsync(_localization.GetMessage("Autochannel name default", _data.Name));
            }

            [Command("set")]
            public async Task AutoChannelNameSet([Remainder] string message)
            {
                _data.Name = message;
                try
                {
                    await _channel.Save(_data);
                }
                catch (InvalidNameException)
                {
                    await ReplyAsync(_localization.GetMessage("Autochannel name invalid empty"));
                    return;
                }

                await ReplyAsync(_localization.GetMessage("Autochannel name set", message));
            }
        }
    }
}