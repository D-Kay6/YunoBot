using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using IDal.Interfaces.Database;
using IDbChannel = IDal.Interfaces.Database.IDbChannel;

namespace Logic.Modules
{
    [Alias("ac")]
    [Group("autochannel")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class AutoChannelModule : ModuleBase<SocketCommandContext>
    {
        private IDbLanguage _language;
        private IDbChannel _channel;
        private Localization.Localization _lang;

        public AutoChannelModule(IDbLanguage language, IDbChannel channel)
        {
            _language = language;
            _channel = channel;
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
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DefaultAutoChannel()
        {
            await ReplyAsync(_lang.GetMessage("Autochannel default"));
        }
        
        [Group("prefix")]
        public class AutoChannelPrefixModule : ModuleBase<SocketCommandContext>
        {
            private IDbLanguage _language;
            private IDbChannel _channel;
            private Localization.Localization _lang;

            public AutoChannelPrefixModule(IDbLanguage language, IDbChannel channel)
            {
                _language = language;
                _channel = channel;
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
            public async Task DefaultAutoChannelPrefix()
            {
                await ReplyAsync(_lang.GetMessage("Autochannel prefix default", await _channel.GetAutoPrefix(Context.Guild.Id)));
            }

            [Command("set")]
            public async Task AutoChannelPrefixSet([Remainder] string message)
            {
                if (message.Equals(await _channel.GetAutoPrefix(Context.Guild.Id), StringComparison.OrdinalIgnoreCase))
                {
                    await ReplyAsync(_lang.GetMessage("Invalid ac/pc prefix"));
                    return;
                }
                await _channel.SetAutoPrefix(Context.Guild.Id, message);
                await ReplyAsync(_lang.GetMessage("Autochannel prefix set", message));
            }
        }
        
        [Group("name")]
        public class AutoChannelNameModule : ModuleBase<SocketCommandContext>
        {
            private IDbLanguage _language;
            private IDbChannel _channel;
            private Localization.Localization _lang;

            public AutoChannelNameModule(IDbLanguage language, IDbChannel channel)
            {
                _language = language;
                _channel = channel;
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
            public async Task DefaultAutoChannelName()
            {
                await ReplyAsync(_lang.GetMessage("Autochannel name default", await _channel.GetAutoName(Context.Guild.Id)));
            }

            [Command("set")]
            public async Task AutoChannelNameSet([Remainder] string message)
            {
                await _channel.SetAutoName(Context.Guild.Id, message);
                await ReplyAsync(_lang.GetMessage("Autochannel name set", message));
            }
        }

        [Command("delete")]
        public async Task AutoChannelDelete()
        {
            var data = await _channel.GetAutoChannel(Context.Guild.Id);
            var channels = Context.Guild.VoiceChannels.Where(c => c.Name.StartsWith(data.Name));
            foreach (var channel in channels)
            {
                if (await _channel.IsGeneratedChannel(Context.Guild.Id, channel.Id)) await _channel.RemoveGeneratedChannel(Context.Guild.Id, channel.Id);
                await channel.DeleteAsync();
            }
            await ReplyAsync(_lang.GetMessage("Autochannel delete", data.Name));
        }
    }
}