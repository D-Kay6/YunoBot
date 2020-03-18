namespace Logic.Handlers
{
    using System.Threading.Tasks;
    using Discord.WebSocket;

    public abstract class BaseHandler
    {
        protected DiscordSocketClient Client;

        protected BaseHandler(DiscordSocketClient client)
        {
            Client = client;
        }

        public abstract Task Initialize();

        public virtual Task Start()
        {
            return Task.CompletedTask;
        }
    }
}