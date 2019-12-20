using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using IDal.Interfaces.Database;
using IDbChannel = IDal.Interfaces.Database.IDbChannel;

namespace Logic.Modules
{
    [Alias("pc")]
    [Group("permachannel")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class PermaChannelModule : ModuleBase<SocketCommandContext>
    {
        private IDbLanguage _language;
        private Localization.Localization _lang;

        public PermaChannelModule(IDbLanguage language)
        {
            _language = language;
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            Task.WaitAll(LoadLanguage());
            base.BeforeExecute(command);
        }

        private async Task LoadLanguage()
        {
            _lang = new Localization.Localization(await _language.GetLanguage(Context.Guild.Id));
        }

        [Command]
        public async Task DefaultPermaChannel()
        {
            await ReplyAsync(_lang.GetMessage("Permachannel default"));
        }

        [Group("prefix")]
        public class PermaChannelPrefixModule : ModuleBase<SocketCommandContext>
        {
            private IDbChannel _channel;
            private IDbLanguage _language;
            private Localization.Localization _lang;

            public PermaChannelPrefixModule(IDbChannel channel, IDbLanguage language)
            {
                _channel = channel;
                _language = language;
            }

            protected override void BeforeExecute(CommandInfo command)
            {
                Task.WaitAll(LoadLanguage());
                base.BeforeExecute(command);
            }

            private async Task LoadLanguage()
            {
                _lang = new Localization.Localization(await _language.GetLanguage(Context.Guild.Id));
            }

            [Command]
            public async Task DefaultPermaChannelPrefix()
            {
                await ReplyAsync(_lang.GetMessage("Permachannel prefix default", await _channel.GetPermaPrefix(Context.Guild.Id)));
            }

            [Command("set")]
            public async Task PermaChannelPrefixSet([Remainder] string message)
            {
                if (message.Equals(await _channel.GetPermaPrefix(Context.Guild.Id), StringComparison.OrdinalIgnoreCase))
                {
                    await ReplyAsync(_lang.GetMessage("Invalid ac/pc prefic"));
                    return;
                }
                await _channel.SetPermaPrefix(Context.Guild.Id, message);
                await ReplyAsync(_lang.GetMessage("Permachannel prefix set", message));
            }
        }

        [Group("name")]
        public class PermaChannelNameModule : ModuleBase<SocketCommandContext>
        {
            private IDbChannel _channel;
            private IDbLanguage _language;
            private Localization.Localization _lang;

            public PermaChannelNameModule(IDbChannel channel, IDbLanguage language)
            {
                _channel = channel;
                _language = language;
            }

            protected override void BeforeExecute(CommandInfo command)
            {
                Task.WaitAll(LoadLanguage());
                base.BeforeExecute(command);
            }

            private async Task LoadLanguage()
            {
                _lang = new Localization.Localization(await _language.GetLanguage(Context.Guild.Id));
            }

            [Command]
            public async Task DefaultPermaChannelName()
            {
                await ReplyAsync(_lang.GetMessage("Permachannel name default", await _channel.GetPermaName(Context.Guild.Id)));
            }

            [Command("set")]
            public async Task PermaChannelNameSet([Remainder] string message)
            {
                await _channel.SetPermaName(Context.Guild.Id, message);
                await ReplyAsync(_lang.GetMessage("Permachannel name set", message));
            }
        }
    }
}