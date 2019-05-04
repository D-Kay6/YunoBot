using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Logic.Extentions;

namespace Logic.Modules
{
    [Group("welcome")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class WelcomeModule : ModuleBase<SocketCommandContext>
    {
        [Priority(-1)]
        [Command]
        public async Task DefaultWelcome([Remainder] string name)
        {
            await Context.Channel.SendMessageAsync($"Wait, who do you mean? I cannot find `{name}`.");
        }

        [Command]
        public async Task DefaultWelcome(SocketGuildUser user)
        {
            await Context.Channel.SendFileAsync(ImageExtentions.GetImagePath("GasaiYunoWelcome.jpg"),
                $"Welcome to the party {user.Mention}. Hope you will have a good time with us.");
        }
    }
}
