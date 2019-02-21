using Discord;
using Discord.WebSocket;
using Logic.Handlers;
using Logic.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using DalFactory;
using IDal.Interfaces;
using ILogic;

namespace Logic
{
    public class Bot : IBot
    {
        private readonly IConfig _config;
        private readonly ISerializer _persistence;
        private readonly ChannelHandler _channelHandler;

        private DiscordSocketClient _client;

        private readonly CommandHandler _commandHandler;
        private readonly DblHandler _dblHandler;
        private readonly RoleHandler _roleHandler;

        private IServiceProvider _services;

        /// <summary>
        ///     Creates a new instance of this class.
        /// </summary>
        public Bot()
        {
            _config = ConfigFactory.GenerateConfig();
            _persistence = SerializerFactory.GenerateSerializer();

            _commandHandler = new CommandHandler();
            _channelHandler = new ChannelHandler();
            _roleHandler = new RoleHandler();
            _dblHandler = new DblHandler();
        }

        /// <summary>
        ///     Asynchronous start of the connection.
        /// </summary>
        public async Task Start()
        {
            while (RestartHandler.KeepAlive)
                try
                {
                    DownloadPrerequisites();
                    var config = _config.Read();
                    if (string.IsNullOrWhiteSpace(config.Token)) return;
                    _client = new DiscordSocketClient(new DiscordSocketConfig
                    {
                        LogLevel = LogSeverity.Verbose
                    });
                    _client.Log += Log;

                    IServiceCollection serviceCollection = new ServiceCollection();
                    ConfigureServices(serviceCollection);
                    _services = serviceCollection.BuildServiceProvider();

                    await _client.LoginAsync(TokenType.Bot, config.Token);
                    await _client.StartAsync();
                    await _client.SetActivityAsync(new Game("her Yukiteru Diary", ActivityType.Watching));

                    await _commandHandler.Initialize(_client, _services);
                    await _channelHandler.Initialize(_client, _services);
                    await _roleHandler.Initialize(_client, _services);
                    await _dblHandler.Initialize(_client, config);

                    await RestartHandler.AwaitRestart();
                }
                catch (Exception e)
                {
                    LogsHandler.Instance.Log("Main", $"Fatal exception occured. Restarting bot. Traceback: {e}");
                }
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(_persistence);
            serviceCollection.AddSingleton(new AudioService(_client));
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

        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
        }
    }
}