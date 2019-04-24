using Discord;
using Discord.Commands;
using Logic.Data;
using System.Threading.Tasks;
using IDal.Interfaces;

namespace Logic.Modules
{
    [Alias("ar")]
    [Group("autorole")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class AutoRoleModule : ModuleBase<SocketCommandContext>
    {
        public ISerializer Persistence { get; set; }
        
        [Command]
        public async Task DefaultAutoRole()
        {
            await ReplyAsync(
@"Auto roles are roles that are temporary granted when a user starts playing a game, and removed when the user stops playing that game.

When setting up auto roles, you need to make sure I have at least the `manage roles` permission.
If you never changed my standard permissions, I should have the `administrator` permission, which works just as well.
Create a role with the auto role prefix (you can get this through `/autorole prefix`) followed by the exact name of the game as it would appear in the status of a user.
You can set-up your own prefix for auto roles with `/autorole prefix set <prefix>`.");
        }

        [Group("prefix")]
        public class AutoChannelPrefixModule : ModuleBase<SocketCommandContext>
        {
            [Command]
            public async Task DefaultAutoRolePrefix()
            {
                var autoRole = AutoRole.Load(Context.Guild.Id);
                await ReplyAsync(
$@"The current auto role prefix is `{autoRole.AutoPrefix}`.
You can check 'http://unicode.org/emoji/charts/full-emoji-list.html' for icons to use in the prefix.");
            }

            [Command("set")]
            public async Task AutoRolePrefixSet([Remainder] string message)
            {
                var autoRole = AutoRole.Load(Context.Guild.Id);
                if (autoRole.SetAutoRoleIcon(message))
                {
                    await ReplyAsync("I am not able to use the same prefix for both auto roles and perma roles.");
                    return;
                }
                autoRole.Save();
                await ReplyAsync($"The new auto role prefix is `{autoRole.AutoPrefix}`.");
            }
        }
    }
}