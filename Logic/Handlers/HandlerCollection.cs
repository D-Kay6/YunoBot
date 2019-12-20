using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Logic.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Logic.Handlers
{
    public class HandlerCollection
    {
        private IServiceProvider _services;
        private Collection<BaseHandler> _handlers;

        public HandlerCollection(IServiceProvider services)
        {
            _services = services;
            _handlers = new Collection<BaseHandler>();

            GenerateHandlers();
        }

        public void GenerateHandlers()
        {
            _handlers.Add(ActivatorUtilities.CreateInstance<StatusHandler>(_services));
            _handlers.Add(ActivatorUtilities.CreateInstance<DatabaseHandler>(_services));
            _handlers.Add(ActivatorUtilities.CreateInstance<BanHandler>(_services));
            _handlers.Add(ActivatorUtilities.CreateInstance<CommandHandler>(_services));
            _handlers.Add(ActivatorUtilities.CreateInstance<DblHandler>(_services));
            _handlers.Add(ActivatorUtilities.CreateInstance<ChannelHandler>(_services));
            _handlers.Add(ActivatorUtilities.CreateInstance<RoleHandler>(_services));
            _handlers.Add(ActivatorUtilities.CreateInstance<WelcomeHandler>(_services));
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
