using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Yuno.Data.Core.Interfaces;
using Yuno.Logic;

namespace Yuno.Main.Commands.Modules
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
            await ReplyAsync($@"The current auto role icon for this server is '{AutoRole.Load(Context.Guild.Id).GetAutoRoleIcon()}'.
You can check 'http://unicode.org/emoji/charts/full-emoji-list.html' for the icon to paste in the channel name.");
        }

        [Command("seticon")]
        public async Task AutoRoleSetIcon([Remainder] string message)
        {
            var persistence = AutoRole.Load(Context.Guild.Id);
            persistence.SetAutoRoleIcon(message);
            persistence.Save();
            await ReplyAsync($"The new auto role icon for this server is '{persistence.GetAutoRoleIcon()}'");
        }
    }
}
