using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Yuno.Data.Core.Interfaces;
using Yuno.Logic;

namespace Yuno.Main.Commands.Modules
{
    [Alias("pr")]
    [Group("permarole")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class PermaRoleModule : ModuleBase<SocketCommandContext>
    {
        public ISerializer Persistence { get; set; }

        [Priority(-1)]
        [Command]
        public async Task DefaultPermaRole()
        {
            await ReplyAsync($@"The current perma role icon for this server is '{AutoRole.Load(Context.Guild.Id).GetPermaRoleIcon()}'.
You can check 'http://unicode.org/emoji/charts/full-emoji-list.html' for the icon to paste in the channel name.");
        }

        [Command("seticon")]
        public async Task PermaRoleSetIcon([Remainder] string message)
        {
            var persistence = AutoRole.Load(Context.Guild.Id);
            persistence.SetPermaRoleIcon(message);
            persistence.Save();
            await ReplyAsync($"The new perma role icon for this server is '{persistence.GetPermaRoleIcon()}'");
        }
    }
}
