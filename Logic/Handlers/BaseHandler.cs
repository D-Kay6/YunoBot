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

        protected BaseHandler(DiscordSocketClient client)
        {
            this.Client = client;
        }

        public abstract Task Initialize();
    }
}
