using Discord.Commands;
using Logic.Extentions;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Group("Invite")]
    public class InviteModule : ModuleBase<SocketCommandContext>
    {
        [Command]
        public async Task DefaultInvite()
        {
            await Context.User.SendDM(@"You can invite me to your server with this link: https://discordapp.com/oauth2/authorize?client_id=286972781273546762&scope=bot&permissions=8.
For more info on me, go take a look at https://discordbots.org/bot/286972781273546762, or use /help in any of the discord servers I'm in.
If you're in need of support, come join my support server at 'https://discord.gg/YFqUMDT'.");
        }
    }
}
