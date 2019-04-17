using Discord;
using Discord.Commands;
using Logic.Data;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Alias("pr")]
    [Group("permarole")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class PermaRoleModule : ModuleBase<SocketCommandContext>
    {
        [Priority(-1)]
        [Command]
        public async Task DefaultPermaRole()
        {
            await ReplyAsync(
                $@"The current perma role prefix for this server is `{AutoRole.Load(Context.Guild.Id).PermaPrefix}`.
You can check 'http://unicode.org/emoji/charts/full-emoji-list.html' for icons to use in the prefix.");
        }

        [Command("seticon")]
        public async Task PermaRoleSetIcon([Remainder] string message)
        {
            var persistence = AutoRole.Load(Context.Guild.Id);
            if (message.Equals(persistence.AutoPrefix))
            {
                await ReplyAsync("I am not able to use the same prefix for both auto roles and perma roles.");
                return;
            }

            persistence.SetPermaRoleIcon(message);
            persistence.Save();
            await ReplyAsync($"The new perma role prefix for this server is `{persistence.PermaPrefix}`");
        }
    }
}