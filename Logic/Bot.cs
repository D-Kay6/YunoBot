using DalFactory;
using Discord;
using Discord.WebSocket;
using ILogic;
using Logic.Handlers;
using Logic.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Logic.Exceptions;

namespace Logic
{
    public class Bot : IBot
    {
        private readonly DiscordSocketClient _client;

        private readonly IServiceProvider _services;

        private readonly HandlerCollection _handlers;

        public Bot()
        {
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
            var configService = _services.GetService<ConfigurationService>();
            await _handlers.Initialize();

            while (restartService.KeepAlive)
            {
                try
                {
                    DownloadPrerequisites();
                    var token = await configService.GetToken();

                    await _handlers.Start();
                    await _client.LoginAsync(TokenType.Bot, token);
                    await _client.StartAsync();

                    await restartService.AwaitRestart();
                }
                catch (InvalidTokenException e)
                {
                    await logsService.Write("Main", "The bot token in the config file could not be read.");
                    break;
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

        public Task Stop()
        {
            var restartService = _services.GetService<RestartService>();
            restartService.Shutdown();
            return Task.CompletedTask;
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

            serviceCollection.AddTransient(serviceProvider => ConfigFactory.GenerateConfig());
            serviceCollection.AddTransient(serviceProvider => LocalizationFactory.GenerateLocalization());

            serviceCollection.AddSingleton(_client);

            serviceCollection.AddSingleton<ConfigurationService>();
            serviceCollection.AddSingleton<LogsService>();
            serviceCollection.AddSingleton<RestartService>();
            serviceCollection.AddSingleton<MusicService>();

            serviceCollection.AddTransient<LocalizationService>();

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

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
            return Task.CompletedTask;
        }
    }
}