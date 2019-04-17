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

        [Priority(-1)]
        [Command]
        public async Task DefaultAutoRole()
        {
            await ReplyAsync(
                $@"The current auto role prefix for this server is `{AutoRole.Load(Context.Guild.Id).AutoPrefix}`.
You can check 'http://unicode.org/emoji/charts/full-emoji-list.html' for icons to use in the prefix.");
        }

        [Command("seticon")]
        public async Task AutoRoleSetIcon([Remainder] string message)
        {
            var persistence = AutoRole.Load(Context.Guild.Id);
            if (message.Equals(persistence.PermaPrefix))
            {
                await ReplyAsync("I am not able to use the same prefix for both auto roles and perma roles.");
                return;
            }

            persistence.SetAutoRoleIcon(message);
            persistence.Save();
            await ReplyAsync($"The new auto role prefix for this server is `{persistence.AutoPrefix}`");
        }
    }
}