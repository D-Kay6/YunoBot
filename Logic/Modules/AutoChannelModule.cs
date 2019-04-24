using System.Linq;
using Discord;
using Discord.Commands;
using Logic.Data;
using System.Threading.Tasks;
using Logic.Extentions;

namespace Logic.Modules
{
    [Alias("ac")]
    [Group("autochannel")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class AutoChannelModule : ModuleBase<SocketCommandContext>
    {
        [Command]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task DefaultAutoChannel()
        {
            await ReplyAsync(
@"Auto channels are a means to keeping your server clean and don't have unnecessary channels like 'voice 1, voice 2, etc'.
These auto channels are voice channels that have a prefix in their name, so I can recognize them.
When a user joins such a voice channel, I will generate a new voice channel with the same permissions and settings in the category the auto channel is in.
I will also give the user permission to modify the channel.
Once everything's done I will move the user to the new channel.
When all users have left the generated voice channel, I will remove it.

When setting up an auto channel, you need to make sure I have at least the `manage channels` permission.
If you never changed my standard permissions, I should have the `administrator` permission, which works just as well.
Create a channel with the auto channel prefix (you can get this through `/autochannel prefix`) followed by a name of your choosing.
You can set-up your own prefix for auto channels with `/autochannel prefix set <prefix>`.

You can customize the name generated channels have with `/autochannel name set <name>`.
You can add `{0}` to fill in the possessive form of the user's name.
Example:
/autochannel name set {0} channel
Result -> D-Kay's channel");
        }
        
        [Group("prefix")]
        public class AutoChannelPrefixModule : ModuleBase<SocketCommandContext>
        {
            [Command]
            public async Task DefaultAutoChannelPrefix()
            {
                var autoChannel = AutoChannel.Load(Context.Guild.Id);
                await ReplyAsync(
$@"The current auto channel prefix is `{autoChannel.AutoPrefix}`.
You can check 'http://unicode.org/emoji/charts/full-emoji-list.html' for icons to use in the prefix.");
            }

            [Command("set")]
            public async Task AutoChannelPrefixSet([Remainder] string message)
            {
                var autoChannel = AutoChannel.Load(Context.Guild.Id);
                if (!autoChannel.SetAutoChannelPrefix(message))
                {
                    await ReplyAsync("I am not able to use the same prefix for both auto channels and perma channels.");
                    return;
                }
                autoChannel.Save();
                await ReplyAsync($"The new auto channel prefix is `{autoChannel.AutoPrefix}`.");
            }
        }
        
        [Group("name")]
        public class AutoChannelNameModule : ModuleBase<SocketCommandContext>
        {
            [Command]
            public async Task DefaultAutoChannelName()
            {
                var autoChannel = AutoChannel.Load(Context.Guild.Id);
                await ReplyAsync($"The current name for auto generated channels is `{autoChannel.AutoName}`.");
            }

            [Command("set")]
            public async Task AutoChannelNameSet([Remainder] string message)
            {
                var autoChannel = AutoChannel.Load(Context.Guild.Id);
                autoChannel.SetAutoChannelName(message);
                autoChannel.Save();
                await ReplyAsync($"The new name for auto generated channels is '{autoChannel.AutoName}'.");
            }
        }

        [Command("fix")]
        public async Task AutoChannelFix()
        {
            AutoChannel.Remove(Context.Guild.Id);
            await ReplyAsync($"Your auto channels should now be fixed. If you had a custom prefix you will need to set this again.");
        }

        [Command("delete")]
        public async Task AutoChannelDelete()
        {
            var autoChannel = AutoChannel.Load(Context.Guild.Id);
            var channels = Context.Guild.VoiceChannels.Where(c => c.Name.StartsWith(autoChannel.AutoName));
            foreach (var channel in channels)
            {
                if (autoChannel.IsControlledChannel(channel.Id)) autoChannel.RemoveChannel(channel.Id);
                await channel.DeleteAsync();
            }
            autoChannel.Save();
            await ReplyAsync($"I have removed any voice channels I could find on your server that had `{autoChannel.AutoName}` as name.");
        }
    }
}