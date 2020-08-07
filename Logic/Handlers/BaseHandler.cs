using Discord.WebSocket;
using Logic.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Logic.Handlers
{
    public abstract class BaseHandler
    {
        protected readonly DiscordShardedClient Client;
        protected readonly LogsService Logs;

        protected BaseHandler(DiscordShardedClient client, LogsService logs)
        {
            Client = client;
            Logs = logs;
        }

        public virtual Task Initialize()
        {
            Client.ShardReady += Ready;

            return Task.CompletedTask;
        }

        public virtual Task Start()
        {
            return Task.CompletedTask;
        }

        public virtual Task Stop()
        {
            return Task.CompletedTask;
        }

        public virtual Task Finish()
        {
            return Task.CompletedTask;
        }

        protected virtual async Task Ready(DiscordSocketClient client)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            try
            {
                while (client?.CurrentUser == null || client.CurrentUser.Id == 0)
                {
                    if (stopwatch.Elapsed > TimeSpan.FromSeconds(30))
                        throw new TimeoutException(
                            $"The ready event handler for {GetType().Name} took too long and has been terminated.");

                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }
            catch (TimeoutException e)
            {
                await Logs.Write("Error", e);
            }
        }
    }
}