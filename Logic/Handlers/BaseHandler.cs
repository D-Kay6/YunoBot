using Discord.WebSocket;
using System.Threading.Tasks;

namespace Logic.Handlers
{
    public abstract class BaseHandler
    {
        protected DiscordSocketClient Client;

        protected BaseHandler(DiscordSocketClient client)
        {
            this.Client = client;
        }

        public abstract Task Initialize();

        public virtual Task Start()
        {
            return Task.CompletedTask;
        }
    }
}