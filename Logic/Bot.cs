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
using Logic.Services.Music;
using Serilog;
using Serilog.Core;

namespace Logic
{
    public class Bot : IBot
    {
        private readonly IConfig _config;

        private DiscordSocketClient _client;

        private readonly UpdateHandler _updateHandler;
        private readonly StatusHandler _statusHandler;
        private readonly DatabaseHandler _databaseHandler;
        private readonly CommandHandler _commandHandler;
        private readonly DblHandler _dblHandler;
        private readonly ChannelHandler _channelHandler;
        private readonly RoleHandler _roleHandler;
        private readonly WelcomeHandler _welcomeHandler;

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

            _services = GenerateServiceProvider();

            _updateHandler = new UpdateHandler(_client, _services);
            _statusHandler = new StatusHandler(_client, _services);
            _databaseHandler = new DatabaseHandler(_client, _services);
            _commandHandler = new CommandHandler(_client, _services);
            _dblHandler = new DblHandler(_client, _services, _config.Read());
            _channelHandler = new ChannelHandler(_client, _services);
            _roleHandler = new RoleHandler(_client, _services);
            _welcomeHandler = new WelcomeHandler(_client, _services);
        }

        /// <summary>
        ///     Asynchronous start of the connection.
        /// </summary>
        public async Task Start()
        {
            var restartService = _services.GetService<RestartService>();

            while (restartService.KeepAlive)
            {
                try
                {
                    DownloadPrerequisites();
                    var config = _config.Read();
                    if (string.IsNullOrWhiteSpace(config.Token)) return;

                    _client.Log += Log;
                    _client.Ready += OnReady;
                    
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
            var log = new LogService();
            var restart = new RestartService();

            serviceCollection.AddSingleton(log);
            serviceCollection.AddSingleton(restart);
            serviceCollection.AddSingleton(new UpdateService(log, restart));
            serviceCollection.AddSingleton(new AudioService(_client));

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
            var shartCount = await _client.GetRecommendedShardCountAsync();
            if (shartCount > 1) LogService.Instance.Log("Main", $"Probably time to think about creating shards. {shartCount}");
            
            await _updateHandler.Initialize();
            await _statusHandler.Initialize();
            await _databaseHandler.Initialize();
            await _commandHandler.Initialize();
            await _dblHandler.Initialize();
            await _channelHandler.Initialize();
            await _roleHandler.Initialize();
            await _welcomeHandler.Initialize();
        }

        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
        }
    }
}