using Logic.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Logic.Handlers
{
    public class HandlerCollection
    {
        private readonly IServiceProvider _services;
        private readonly Collection<BaseHandler> _handlers;

        public HandlerCollection(IServiceProvider services)
        {
            _services = services;
            _handlers = new Collection<BaseHandler>();

            GenerateHandlers();
        }

        public void GenerateHandlers()
        {
            CreateInstance<StatusHandler>();
            CreateInstance<DatabaseHandler>();
            CreateInstance<BanHandler>();
            CreateInstance<CommandHandler>();
            CreateInstance<DblHandler>();
            CreateInstance<ChannelHandler>();
            CreateInstance<RoleHandler>();
            CreateInstance<WelcomeHandler>();
            CreateInstance<MusicHandler>();
        }

        private void CreateInstance<T>() where T : BaseHandler
        {
            _handlers.Add(ActivatorUtilities.CreateInstance<T>(_services));
        }

        public async Task Initialize()
        {
            foreach (var handler in _handlers)
            {
                await handler.Initialize();
            }
        }
    }
}