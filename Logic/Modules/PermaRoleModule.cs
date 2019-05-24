using DalFactory;
using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Alias("pr")]
    [Group("permarole")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class PermaRoleModule : ModuleBase<SocketCommandContext>
    {
        [Command]
        public async Task DefaultPermaRole()
        {
            await ReplyAsync(
@"Perma roles are roles that are permanently granted when a user starts playing a game.

When setting up perma roles, you need to make sure I have at least the `manage roles` permission.
If you never changed my standard permissions, I should have the `administrator` permission, which works just as well.
Create a role with the perma role prefix (you can get this through `/permarole prefix`) followed by the exact name of the game as it would appear in the status of a user.
You can set-up your own prefix for perma roles with `/permarole prefix set <prefix>`.");
        }

        [Group("prefix")]
        public class PermaChannelPrefixModule : ModuleBase<SocketCommandContext>
        {
            [Command]
            public async Task DefaultPermaRolePrefix()
            {
                var autoRole = DatabaseFactory.GenerateAutoRole();
                await ReplyAsync(
$@"The current perma role prefix for this server is `{autoRole.GetData(Context.Guild.Id).PermaPrefix}`.
You can check 'http://unicode.org/emoji/charts/full-emoji-list.html' for icons to use in the prefix.");
            }

            [Command("set")]
            public async Task PermaRolePrefixSet([Remainder] string message)
            {
                var autoRole = DatabaseFactory.GenerateAutoRole();
                var data = autoRole.GetData(Context.Guild.Id);
                if (message.Equals(data.AutoPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    await ReplyAsync("I am not able to use the same prefix for both auto roles and perma roles.");
                    return;
                }
                autoRole.SetPermaPrefix(Context.Guild.Id, message);
                await ReplyAsync($"The new perma role prefix is `{message}`.");
            }
        }
    }
}