using DalFactory;
using Discord;
using Discord.WebSocket;
using IDal.Interfaces;
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

        /// <summary>
        ///     Creates a new instance of this class.
        /// </summary>
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

        /// <summary>
        ///     Asynchronous start of the connection.
        /// </summary>
        public async Task Start()
        {
            var restartService = _services.GetService<RestartService>();
            await _handlers.Initialize();

            while (restartService.KeepAlive)
            {
                try
                {
                    DownloadPrerequisites();
                    var config = _config.Read();
                    if (string.IsNullOrWhiteSpace(config.Token)) return;
                    
                    await _client.LoginAsync(TokenType.Bot, config.Token);
                    await _client.StartAsync();

                    await restartService.AwaitRestart();
                }
                catch (Exception e)
                {
                    LogService.Instance.Log("Main", $"Fatal exception occured. Restarting bot. Traceback: {e}");
                }
                finally
                {
                    await _client.StopAsync();
                }
            }
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
            serviceCollection.AddSingleton(_config.Read());

            serviceCollection.AddSingleton(new LogService());
            serviceCollection.AddSingleton(new RestartService());
            serviceCollection.AddSingleton(new AudioService(_client, DatabaseFactory.GenerateLanguage()));

            return serviceCollection.BuildServiceProvider();

            //serviceCollection.AddSingleton(DatabaseFactory.GenerateServer());
            //serviceCollection.AddSingleton(DatabaseFactory.GenerateUser());
            //serviceCollection.AddSingleton(DatabaseFactory.GenerateBan());
            //serviceCollection.AddSingleton(DatabaseFactory.GenerateLanguage());
            //serviceCollection.AddSingleton(DatabaseFactory.GenerateCommand());
            //serviceCollection.AddSingleton(DatabaseFactory.GenerateWelcome());
            //serviceCollection.AddSingleton(DatabaseFactory.GenerateChannel());
            //serviceCollection.AddSingleton(DatabaseFactory.GenerateRole());
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
            var shartCount = await _client.GetRecommendedShardCountAsync();
            if (shartCount > 1) LogService.Instance.Log("Main", $"Probably time to think about creating shards. {shartCount}");
        }

        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
        }
    }
}