using Discord;
using Discord.Commands;
using Logic.Data;
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
            await ReplyAsync(
                $"The prefix for {Context.Guild.Name} is '{CommandSettings.Load(Context.Guild.Id).Prefix}'.");
        }

        [Command("set")]
        public async Task PrefixSet([Remainder] string message)
        {
            var persistence = CommandSettings.Load(Context.Guild.Id);
            persistence.ChangePrefix(message);
            await ReplyAsync($"The prefix for {Context.Guild.Name} was changed to '{message}'.");
            persistence.Save();
        }
    }
}