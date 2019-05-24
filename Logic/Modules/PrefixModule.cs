using DalFactory;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace Logic.Modules
{
    [Group("prefix")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class PrefixModule : ModuleBase<SocketCommandContext>
    {
        [Command]
        public async Task DefaultPrefix()
        {
            var settings = DatabaseFactory.GenerateServerSettings();
            await ReplyAsync($"The prefix for {Context.Guild.Name} is `{settings.GetCommandPrefix(Context.Guild.Id)}`.");
        }

        [Command("set")]
        public async Task PrefixSet([Remainder] string message)
        {
            var settings = DatabaseFactory.GenerateServerSettings();
            settings.SetCommandPrefix(Context.Guild.Id, message);
            await ReplyAsync($"The prefix for {Context.Guild.Name} was changed to `{message}`.");
        }
    }
}