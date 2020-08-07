using Discord;
using Discord.WebSocket;
using Logic.Services;
using System.Threading.Tasks;

namespace Logic.Handlers
{
    public class ReactionRoleHandler : BaseHandler
    {
        public ReactionRoleHandler(DiscordShardedClient client, LogsService logs) : base(client, logs)
        {
        }

        public override Task Initialize()
        {
            base.Initialize();
            Client.ReactionAdded += OnReactionAdded;
            Client.ReactionRemoved += OnReactionRemoved;

            return Task.CompletedTask;
        }

        private async Task OnReactionRemoved(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
#if DEBUG
            if (!arg3.UserId.Equals(255453041531158538)) return;
#endif
            arg3.Emote.
        }

        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
#if DEBUG
            if (!arg3.UserId.Equals(255453041531158538)) return;
#endif
        }
    }
}