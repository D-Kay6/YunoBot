using Discord.WebSocket;
using System.Threading.Tasks;

namespace Logic.Handlers
{
    public abstract class BaseHandler
    {
        private bool _loaded;

        protected DiscordSocketClient Client;

        protected BaseHandler(DiscordSocketClient client)
        {
            this.Client = client;
        }

        public abstract Task Initialize();

        protected void FinishLoading()
        {
            _loaded = true;
        }

        protected bool IsLoaded()
        {
            return _loaded;
        }
    }
}
