using Discord.Commands;
using System.Threading.Tasks;
using Discord;
using Yuno.Data.Core.Interfaces;
using Yuno.Logic;

namespace Yuno.Main.Commands.Modules
{
    [Group("prefix")]
    [RequireUserPermission(GuildPermission.Administrator)]
    public class PrefixModule : ModuleBase<SocketCommandContext>
    {
        public ISerializer Persistence { get; set; }

        [Command]
        public async Task DefaultPrefix()
        {
            await ReplyAsync($"The prefix for {Context.Guild.Name} is '{CommandSettings.Load(Context.Guild.Id).Prefix}'.");
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
