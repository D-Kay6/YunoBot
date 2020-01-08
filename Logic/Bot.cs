using DalFactory;
using Discord;
using Discord.WebSocket;
using IDal;
using ILogic;
using Logic.Handlers;
using Logic.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Logic
{
    public class Bot : IBot
    {
        private readonly IConfig _config;

        private DiscordSocketClient _client;

        private readonly HandlerCollection _handlers;

        private IServiceProvider _services;

        public Bot()
        {
            _config = ConfigFactory.GenerateConfig();
            
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });
            _client.Log += Log;
            _client.Ready += OnReady;

            _services = GenerateServiceProvider();

            _handlers = new HandlerCollection(_services);
        }

        public async Task Start()
        {
            var restartService = _services.GetService<RestartService>();
            var logsService = _services.GetService<LogsService>();
            await _handlers.Initialize();

            while (restartService.KeepAlive)
            {
                try
                {
                    DownloadPrerequisites();
                    var config = await _config.Read();
                    if (string.IsNullOrWhiteSpace(config.Token)) return;
                    
                    await _client.LoginAsync(TokenType.Bot, config.Token);
                    await _client.StartAsync();

                    await restartService.AwaitRestart();
                }
                catch (Exception e)
                {
                    await logsService.Write("Main", $"Fatal exception occured. Restarting bot. Traceback: {e}");
                }
                finally
                {
                    await _client.StopAsync();
                }
            }
        }

        public async Task Stop()
        {
            var restartService = _services.GetService<RestartService>();
            restartService.Shutdown();
        }

        private ServiceProvider GenerateServiceProvider()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddTransient(serviceProvider => DatabaseFactory.GenerateServer());
            serviceCollection.AddTransient(serviceProvider => DatabaseFactory.GenerateUser());
            serviceCollection.AddTransient(serviceProvider => DatabaseFactory.GenerateBan());
            serviceCollection.AddTransient(serviceProvider => DatabaseFactory.GenerateLanguage());
            serviceCollection.AddTransient(serviceProvider => DatabaseFactory.GenerateCommand());
            serviceCollection.AddTransient(serviceProvider => DatabaseFactory.GenerateWelcome());
            serviceCollection.AddTransient(serviceProvider => DatabaseFactory.GenerateChannel());
            serviceCollection.AddTransient(serviceProvider => DatabaseFactory.GenerateRole());

            serviceCollection.AddSingleton(_client);
            serviceCollection.AddSingleton(_config);

            var logsService = new LogsService();
            serviceCollection.AddTransient<LocalizationService>();
            serviceCollection.AddSingleton(logsService);
            serviceCollection.AddSingleton(new RestartService(logsService));
            serviceCollection.AddSingleton(new MusicService(_client, DatabaseFactory.GenerateLanguage(), new LocalizationService()));

            return serviceCollection.BuildServiceProvider();
        }

        private void DownloadPrerequisites()
        {
            using (var client = new WebClient())
            {
                var file = "libsodium.dll";
                if (!File.Exists(file)) client.DownloadFile("https://discord.foxbot.me/binaries/win64/libsodium.dll", file);

                file = "opus.dll";
                if (!File.Exists(file)) client.DownloadFile("https://discord.foxbot.me/binaries/win64/opus.dll", file);
            }
        }

        private async Task OnReady()
        {
            var logService = _services.GetService<LogsService>();
            var shardCount = await _client.GetRecommendedShardCountAsync();
            if (shardCount > 1) await logService.Write("Main", $"Probably time to think about creating shards. {shardCount}");
        }

        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
        }
    }
}