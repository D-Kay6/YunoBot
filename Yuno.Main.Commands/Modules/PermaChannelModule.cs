using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Yuno.Data.Core.Interfaces;
using Yuno.Logic;

namespace Yuno.Main.Commands.Modules
{
    [Alias("pc")]
    [Group("permachannel")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class PermaChannelModule : ModuleBase<SocketCommandContext>
    {
        public ISerializer Persistence { get; set; }

        [Priority(-1)]
        [Command]
        public async Task DefaultPermaChannel()
        {
            await ReplyAsync($@"The current perma channel icon for this server is '{AutoChannel.Load(Context.Guild.Id).GetPermaChannelIcon()}'.
You can check 'http://unicode.org/emoji/charts/full-emoji-list.html' for the icon to paste in the channel name.");
        }

        [Command("seticon")]
        public async Task PermaChannelSetIcon([Remainder] string message)
        {
            var persistence = AutoChannel.Load(Context.Guild.Id);
            persistence.SetPermaChannelIcon(message);
            persistence.Save();
            await ReplyAsync($"The new perma channel icon for this server is '{persistence.GetPermaChannelIcon()}'");
        }
    }
}
