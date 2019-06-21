using DalFactory;
using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

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
            private Localization.Localization _lang;

            protected override void BeforeExecute(CommandInfo command)
            {
                _lang = new Localization.Localization(Context.Guild.Id);
                base.BeforeExecute(command);
            }

            [Command]
            public async Task DefaultPermaChannelPrefix()
            {
                var autoChannel = DatabaseFactory.GenerateAutoChannel();
                await ReplyAsync(_lang.GetMessage("Permachannel prefix default", autoChannel.GetData(Context.Guild.Id).PermaPrefix));
            }

            [Command("set")]
            public async Task PermaChannelPrefixSet([Remainder] string message)
            {
                var autoChannel = DatabaseFactory.GenerateAutoChannel();
                var data = autoChannel.GetData(Context.Guild.Id);
                if (message.Equals(data.AutoPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    await ReplyAsync(_lang.GetMessage("Invalid ac/pc prefic"));
                    return;
                }
                autoChannel.SetPermaPrefix(Context.Guild.Id, message);
                await ReplyAsync(_lang.GetMessage("Permachannel prefix set", message));
            }
        }

        [Group("name")]
        public class PermaChannelNameModule : ModuleBase<SocketCommandContext>
        {
            private Localization.Localization _lang;

            protected override void BeforeExecute(CommandInfo command)
            {
                _lang = new Localization.Localization(Context.Guild.Id);
                base.BeforeExecute(command);
            }

            [Command]
            public async Task DefaultPermaChannelName()
            {
                var autoChannel = DatabaseFactory.GenerateAutoChannel();
                await ReplyAsync(_lang.GetMessage("Permachannel name default", autoChannel.GetData(Context.Guild.Id).PermaName));
            }

            [Command("set")]
            public async Task PermaChannelNameSet([Remainder] string message)
            {
                var autoChannel = DatabaseFactory.GenerateAutoChannel();
                autoChannel.SetPermaName(Context.Guild.Id, message);
                await ReplyAsync(_lang.GetMessage("Permachannel name set", message));
            }
        }
    }
}