using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using IDal.Interfaces.Database;
using IChannel = IDal.Interfaces.Database.IChannel;

namespace Logic.Modules
{
    [Alias("ac")]
    [Group("autochannel")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class AutoChannelModule : ModuleBase<SocketCommandContext>
    {
        private ILanguage _language;
        private IChannel _channel;
        private Localization.Localization _lang;

        public AutoChannelModule(ILanguage language, IChannel channel)
        {
            _language = language;
            _channel = channel;
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            _lang = new Localization.Localization(_language.GetLanguage(Context.Guild.Id));
            base.BeforeExecute(command);
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
            private ILanguage _language;
            private IChannel _channel;
            private Localization.Localization _lang;

            public AutoChannelPrefixModule(ILanguage language, IChannel channel)
            {
                _language = language;
                _channel = channel;
            }

            protected override void BeforeExecute(CommandInfo command)
            {
                _lang = new Localization.Localization(_language.GetLanguage(Context.Guild.Id));
                base.BeforeExecute(command);
            }

            [Command]
            public async Task DefaultAutoChannelPrefix()
            {
                await ReplyAsync(_lang.GetMessage("Autochannel prefix default", _channel.GetAutoPrefix(Context.Guild.Id)));
            }

            [Command("set")]
            public async Task AutoChannelPrefixSet([Remainder] string message)
            {
                if (message.Equals(_channel.GetAutoPrefix(Context.Guild.Id), StringComparison.OrdinalIgnoreCase))
                {
                    await ReplyAsync(_lang.GetMessage("Invalid ac/pc prefix"));
                    return;
                }
                _channel.SetAutoPrefix(Context.Guild.Id, message);
                await ReplyAsync(_lang.GetMessage("Autochannel prefix set", message));
            }
        }
        
        [Group("name")]
        public class AutoChannelNameModule : ModuleBase<SocketCommandContext>
        {
            private ILanguage _language;
            private IChannel _channel;
            private Localization.Localization _lang;

            public AutoChannelNameModule(ILanguage language, IChannel channel)
            {
                _language = language;
                _channel = channel;
            }

            protected override void BeforeExecute(CommandInfo command)
            {
                _lang = new Localization.Localization(_language.GetLanguage(Context.Guild.Id));
                base.BeforeExecute(command);
            }

            [Command]
            public async Task DefaultAutoChannelName()
            {
                await ReplyAsync(_lang.GetMessage("Autochannel name default", _channel.GetAutoName(Context.Guild.Id)));
            }

            [Command("set")]
            public async Task AutoChannelNameSet([Remainder] string message)
            {
                _channel.SetAutoName(Context.Guild.Id, message);
                await ReplyAsync(_lang.GetMessage("Autochannel name set", message));
            }
        }

        [Command("delete")]
        public async Task AutoChannelDelete()
        {
            var data = _channel.GetAutoChannel(Context.Guild.Id);
            var channels = Context.Guild.VoiceChannels.Where(c => c.Name.StartsWith(data.Name));
            foreach (var channel in channels)
            {
                if (_channel.IsGeneratedChannel(Context.Guild.Id, channel.Id)) _channel.RemoveGeneratedChannel(Context.Guild.Id, channel.Id);
                await channel.DeleteAsync();
            }
            await ReplyAsync(_lang.GetMessage("Autochannel delete", data.Name));
        }
    }
}