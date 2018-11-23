using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Yuno.Data.Core.Interfaces;
using Yuno.Logic;

namespace Yuno.Main.Commands.Modules
{
    [Alias("ac")]
    [Group("autochannel")]
    public class AutoChannelModule : ModuleBase<SocketCommandContext>
    {
        public ISerializer Persistence { get; set; }

        [Priority(-1)]
        [Command]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DefaultAutoChannel([Remainder] string message = null)
        {
            if (!string.IsNullOrWhiteSpace(message))
            {
                await ReplyAsync($"There is no sub-command '{message}' in /autochannel.");
                return;
            }
            await ReplyAsync($"The channel icon for your server is '{AutoChannel.Load(Context.Guild.Id).GetAutoChannelIcon()}'");
        }

        [Command("seticon")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AutoChannelSetIcon([Remainder] string message)
        {
            var persistence = AutoChannel.Load(Context.Guild.Id);
            persistence.SetAutoChannelIcon(message);
            persistence.Save();
            await ReplyAsync($"The new auto channel icon for your server is '{persistence.GetAutoChannelIcon()}'");
        }

        [Command("enable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AutoChannelEnable([Remainder] string message)
        {
            var persistence = AutoChannel.Load(Context.Guild.Id);
            persistence.SetAutoChannelIcon(message);
            persistence.Save();
            await ReplyAsync($"The new auto channel icon for your server is '{persistence.GetAutoChannelIcon()}'");
        }

        [Command("disable")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task AutoChannelDisable([Remainder] string message)
        {
            var persistence = AutoChannel.Load(Context.Guild.Id);
            persistence.SetAutoChannelIcon(message);
            persistence.Save();
            await ReplyAsync($"The new auto channel icon for your server is '{persistence.GetAutoChannelIcon()}'");
        }
    }
}
