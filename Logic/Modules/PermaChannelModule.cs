using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using IChannel = IDal.Interfaces.Database.IChannel;

namespace Logic.Modules
{
    [Alias("pc")]
    [Group("permachannel")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class PermaChannelModule : ModuleBase<SocketCommandContext>
    {
        private Localization.Localization _lang;

        protected override void BeforeExecute(CommandInfo command)
        {
            _lang = new Localization.Localization(Context.Guild.Id);
            base.BeforeExecute(command);
        }

        [Command]
        public async Task DefaultPermaChannel()
        {
            await ReplyAsync(_lang.GetMessage("Permachannel default"));
        }

        [Group("prefix")]
        public class PermaChannelPrefixModule : ModuleBase<SocketCommandContext>
        {
            private IChannel _channel;
            private Localization.Localization _lang;

            public PermaChannelPrefixModule(IChannel channel)
            {
                _channel = channel;
            }

            protected override void BeforeExecute(CommandInfo command)
            {
                _lang = new Localization.Localization(Context.Guild.Id);
                base.BeforeExecute(command);
            }

            [Command]
            public async Task DefaultPermaChannelPrefix()
            {
                await ReplyAsync(_lang.GetMessage("Permachannel prefix default", _channel.GetPermaPrefix(Context.Guild.Id)));
            }

            [Command("set")]
            public async Task PermaChannelPrefixSet([Remainder] string message)
            {
                if (message.Equals(_channel.GetPermaPrefix(Context.Guild.Id), StringComparison.OrdinalIgnoreCase))
                {
                    await ReplyAsync(_lang.GetMessage("Invalid ac/pc prefic"));
                    return;
                }
                _channel.SetPermaPrefix(Context.Guild.Id, message);
                await ReplyAsync(_lang.GetMessage("Permachannel prefix set", message));
            }
        }

        [Group("name")]
        public class PermaChannelNameModule : ModuleBase<SocketCommandContext>
        {
            private IChannel _channel;
            private Localization.Localization _lang;

            public PermaChannelNameModule(IChannel channel)
            {
                _channel = channel;
            }

            protected override void BeforeExecute(CommandInfo command)
            {
                _lang = new Localization.Localization(Context.Guild.Id);
                base.BeforeExecute(command);
            }

            [Command]
            public async Task DefaultPermaChannelName()
            {
                await ReplyAsync(_lang.GetMessage("Permachannel name default", _channel.GetPermaName(Context.Guild.Id)));
            }

            [Command("set")]
            public async Task PermaChannelNameSet([Remainder] string message)
            {
                _channel.SetPermaName(Context.Guild.Id, message);
                await ReplyAsync(_lang.GetMessage("Permachannel name set", message));
            }
        }
    }
}