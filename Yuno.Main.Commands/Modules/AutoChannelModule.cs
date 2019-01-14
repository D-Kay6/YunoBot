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
    [RequireUserPermission(GuildPermission.Administrator)]
    public class AutoChannelModule : ModuleBase<SocketCommandContext>
    {
        [Priority(-1)]
        [Command]
        public async Task DefaultAutoChannel()
        {
            await ReplyAsync($@"The current auto channel icon for this server is '{AutoChannel.Load(Context.Guild.Id).GetAutoChannelIcon()}'.
You can check 'http://unicode.org/emoji/charts/full-emoji-list.html' for the icon to paste in the channel name.");
        }

        [Command("seticon")]
        public async Task AutoChannelSetIcon([Remainder] string message)
        {
            var persistence = AutoChannel.Load(Context.Guild.Id);
            persistence.SetAutoChannelIcon(message);
            persistence.Save();
            await ReplyAsync($"The new auto channel icon for this server is '{persistence.GetAutoChannelIcon()}'");
        }
    }
}
