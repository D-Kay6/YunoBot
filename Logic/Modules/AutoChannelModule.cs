using Discord;
using Discord.Commands;
using Logic.Data;
using System.Threading.Tasks;

namespace Logic.Modules
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
            await ReplyAsync(
                $@"The current auto channel prefix for this server is `{AutoChannel.Load(Context.Guild.Id).AutoPrefix}`.
You can check 'http://unicode.org/emoji/charts/full-emoji-list.html' for icons to use in the prefix.");
        }

        [Command("seticon")]
        public async Task AutoChannelSetIcon([Remainder] string message)
        {
            var persistence = AutoChannel.Load(Context.Guild.Id);
            if (message.Equals(persistence.PermaPrefix))
            {
                await ReplyAsync("I am not able to use the same prefix for both auto channels and perma channels.");
                return;
            }

            persistence.SetAutoChannelIcon(message);
            persistence.Save();
            await ReplyAsync($"The new auto channel prefix for this server is `{persistence.AutoPrefix}`");
        }

        [Command("fix")]
        public async Task AutoChannelFix()
        {
            AutoChannel.Remove(Context.Guild.Id);
            await ReplyAsync($"Your auto channels should now be fixed. If you had a custom prefix you will need to set this again.");
        }
    }
}