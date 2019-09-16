using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace Logic.Handlers
{
    public abstract class BaseHandler
    {
        protected DiscordSocketClient Client;
        protected IServiceProvider ServiceProvider;

        protected BaseHandler(DiscordSocketClient client, IServiceProvider serviceProvider)
        {
            this.Client = client;
            this.ServiceProvider = serviceProvider;
        }

        public abstract Task Initialize();
    }
}
