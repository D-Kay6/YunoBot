using System.Linq;
using Discord;
using Discord.Commands;
using Logic.Data;
using System.Threading.Tasks;
using Logic.Extentions;

namespace Logic.Modules
{
    [Alias("pc")]
    [Group("permachannel")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class PermaChannelModule : ModuleBase<SocketCommandContext>
    {
        [Command]
        public async Task DefaultPermaChannel()
        {
            await ReplyAsync(
@"Perma channels are for when you want users to be able to have their own channels without having to bother admins.
These perma channels are voice channels that have a prefix in their name, so I can recognize them.
When a user joins such a voice channel, I will generate a new voice channel with the same permissions and settings in the category the perma channel is in.
I will also give the user permission to modify the channel.
Once everything's done I will move the user to the new channel.
Though, unlike auto channels, I will not remove these channels.

When setting up a perma channel, you need to make sure I have at least the `manage channels` permission.
If you never changed my standard permissions, I should have the `administrator` permission, which works just as well.
Create a channel with the perma channel prefix (you can get this through `/permachannel prefix`) followed by a name of your choosing.
You can set-up your own prefix for perma channels with `/permachannel prefix set <prefix>`.

You can customize the name generated channels have with `/permachannel name set <name>`.
You can add `{0}` to fill in the possessive form of the user's name.
Example:
/permachannel name set {0} channel
Result -> D-Kay's channel");
        }

        [Group("prefix")]
        public class PermaChannelPrefixModule : ModuleBase<SocketCommandContext>
        {
            [Command]
            public async Task DefaultPermaChannelPrefix()
            {
                var autoChannel = AutoChannel.Load(Context.Guild.Id);
                await ReplyAsync(
$@"The current perma channel prefix for this server is '{autoChannel.PermaPrefix}'.
You can check 'http://unicode.org/emoji/charts/full-emoji-list.html' for icons to use in the prefix.");
            }

            [Command("set")]
            public async Task PermaChannelPrefixSet([Remainder] string message)
            {
                var autoChannel = AutoChannel.Load(Context.Guild.Id);
                if (autoChannel.SetPermaChannelPrefix(message))
                {
                    await ReplyAsync("I am not able to use the same prefix for both auto channels and perma channels.");
                    return;
                }
                autoChannel.Save();
                await ReplyAsync($"The new perma channel prefix is '{autoChannel.PermaPrefix}'.");
            }
        }

        [Group("name")]
        public class PermaChannelNameModule : ModuleBase<SocketCommandContext>
        {
            [Command]
            public async Task DefaultPermaChannelName()
            {
                var autoChannel = AutoChannel.Load(Context.Guild.Id);
                await ReplyAsync($"The current name for perma generated channels is `{autoChannel.AutoName}`.");
            }

            [Command("set")]
            public async Task PermaChannelNameSet([Remainder] string message)
            {
                var autoChannel = AutoChannel.Load(Context.Guild.Id);
                autoChannel.SetPermaChannelName(message);
                autoChannel.Save();
                await ReplyAsync($"The new name for perma generated channels is '{autoChannel.PermaName}'.");
            }
        }
    }
}