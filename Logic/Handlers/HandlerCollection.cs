﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;
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
            CreateInstance<DatabaseHandler>();
            CreateInstance<BanHandler>();
            CreateInstance<CommandHandler>();
            CreateInstance<DblHandler>();
            CreateInstance<ChannelHandler>();
            CreateInstance<RoleHandler>();
            CreateInstance<WelcomeHandler>();
            CreateInstance<MusicHandler>();
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