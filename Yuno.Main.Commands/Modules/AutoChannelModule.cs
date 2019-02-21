using Discord;
using Discord.Commands;
using System.Threading.Tasks;
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
            if (message.StartsWith(persistence.GetPermaChannelIcon()))
            {
                await ReplyAsync($"I am not able to use the same icon for both auto channels and perma channels.");
                return;
            }
            persistence.SetAutoChannelIcon(message);
            persistence.Save();
            await ReplyAsync($"The new auto channel icon for this server is '{persistence.GetAutoChannelIcon()}'");
        }

        [Command("fix")]
        public async Task AutoChannelFix()
        {
            AutoChannel.Remove(Context.Guild.Id);
            await ReplyAsync($"Your auto channels should now be fixed. If you had a custom prefix you will need to set this again.");
        }
    }
}
