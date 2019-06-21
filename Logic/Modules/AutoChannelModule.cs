using DalFactory;
using Discord;
using Discord.Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Alias("ac")]
    [Group("autochannel")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class AutoChannelModule : ModuleBase<SocketCommandContext>
    {
        private Localization.Localization _lang;

        protected override void BeforeExecute(CommandInfo command)
        {
            _lang = new Localization.Localization(Context.Guild.Id);
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
            private Localization.Localization _lang;

            protected override void BeforeExecute(CommandInfo command)
            {
                _lang = new Localization.Localization(Context.Guild.Id);
                base.BeforeExecute(command);
            }

            [Command]
            public async Task DefaultAutoChannelPrefix()
            {
                var autoChannel = DatabaseFactory.GenerateAutoChannel();
                await ReplyAsync(_lang.GetMessage("Autochannel prefix default", autoChannel.GetData(Context.Guild.Id).AutoPrefix));
            }

            [Command("set")]
            public async Task AutoChannelPrefixSet([Remainder] string message)
            {
                var autoChannel = DatabaseFactory.GenerateAutoChannel();
                var data = autoChannel.GetData(Context.Guild.Id);
                if (message.Equals(data.PermaPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    await ReplyAsync(_lang.GetMessage("Invalid ac/pc prefix"));
                    return;
                }
                autoChannel.SetAutoPrefix(Context.Guild.Id, message);
                await ReplyAsync(_lang.GetMessage("Autochannel prefix information", message));
            }
        }
        
        [Group("name")]
        public class AutoChannelNameModule : ModuleBase<SocketCommandContext>
        {
            private Localization.Localization _lang;

            protected override void BeforeExecute(CommandInfo command)
            {
                _lang = new Localization.Localization(Context.Guild.Id);
                base.BeforeExecute(command);
            }

            [Command]
            public async Task DefaultAutoChannelName()
            {
                var autoChannel = DatabaseFactory.GenerateAutoChannel();
                await ReplyAsync(_lang.GetMessage("Autochannel name default", autoChannel.GetData(Context.Guild.Id).AutoName));
            }

            [Command("set")]
            public async Task AutoChannelNameSet([Remainder] string message)
            {
                var autoChannel = DatabaseFactory.GenerateAutoChannel();
                autoChannel.SetAutoName(Context.Guild.Id, message);
                await ReplyAsync(_lang.GetMessage("Autochannel name set", message));
            }
        }

        [Command("delete")]
        public async Task AutoChannelDelete()
        {
            var autoChannel = DatabaseFactory.GenerateAutoChannel();
            var data = autoChannel.GetData(Context.Guild.Id);
            var channels = Context.Guild.VoiceChannels.Where(c => c.Name.StartsWith(data.AutoName));
            foreach (var channel in channels)
            {
                if (autoChannel.IsGeneratedChannel(Context.Guild.Id, channel.Id)) autoChannel.RemoveGeneratedChannel(Context.Guild.Id, channel.Id);
                await channel.DeleteAsync();
            }
            await ReplyAsync(_lang.GetMessage("Autochannel delete", data.AutoName));
        }
    }
}