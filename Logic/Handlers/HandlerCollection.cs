namespace Logic.Handlers
{
    using System;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;

    public class HandlerCollection
    {
        private readonly Collection<BaseHandler> _handlers;
        private readonly IServiceProvider _services;

        public HandlerCollection(IServiceProvider services)
        {
            _services = services;
            _handlers = new Collection<BaseHandler>();

            GenerateHandlers();
        }

        public void GenerateHandlers()
        {
            CreateInstance<LoggingHandler>();
            CreateInstance<DatabaseHandler>();
            CreateInstance<BanHandler>();
            CreateInstance<CommandHandler>();
            CreateInstance<DblHandler>();
            CreateInstance<MusicHandler>();
            CreateInstance<ChannelHandler>();
            CreateInstance<RoleHandler>();
            CreateInstance<WelcomeHandler>();
            CreateInstance<StatusHandler>();
        }

        private void CreateInstance<T>() where T : BaseHandler
        {
            _handlers.Add(ActivatorUtilities.CreateInstance<T>(_services));
        }

        public async Task Initialize()
        {
            foreach (var handler in _handlers)
            {
                Console.WriteLine($"Loading {handler.GetType().Name}.");
                await handler.Initialize();
            }
        }

        public async Task Start()
        {
            foreach (var handler in _handlers)
            {
                Console.WriteLine($"Starting {handler.GetType().Name}.");
                await handler.Start();
            }
        }
    }
}