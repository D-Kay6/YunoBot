using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Yuno.Data.Core.Interfaces;
using Yuno.Data.Factory;
using Yuno.Main.AutoChannel;
using Yuno.Main.AutoRole;
using Yuno.Main.Commands;
using Yuno.Main.Core;
using Yuno.Main.DiscordBotList;
using Yuno.Main.Logging;
using Yuno.Main.Music.YouTube;
using Yuno.Main.Restart;

namespace Yuno.Main
{
    public class Bot : IBot
    {
        private readonly IConfig _config;
        private readonly ISerializer _persistence;

        private DiscordSocketClient _client;

        private CommandHandler _commandHandler;
        private ChannelHandler _channelHandler;
        private RoleHandler _roleHandler;
        private DblHandler _dblHandler;

        private IServiceProvider _services;

        /// <summary>
        /// Creates a new instance of this class.
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
        /// Asynchronous start of the connection.
        /// </summary>
        public async Task Start()
        {
            while (RestartHandler.KeepAlive)
            {
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
                    await _client.SetActivityAsync(new Game("with her Yukiteru Diary"));

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
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(new YouTubeDownloadService());
            serviceCollection.AddSingleton(_persistence);
        }

        private void DownloadPrerequisites()
        {
            using (var client = new WebClient())
            {
                var directory = "Bin";
                var file = "Bin/Youtube-dl.exe";
                if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
                if (File.Exists(Path.Combine(directory, file))) File.Delete(Path.Combine(directory, file));
                client.DownloadFile("https://youtube-dl.org/downloads/latest/youtube-dl.exe", file);
            }
            using (var client = new WebClient())
            {
                var file = "libsodium.dll";
                if (!File.Exists(file)) client.DownloadFile("https://discord.foxbot.me/binaries/win64/libsodium.dll", file);
            }
            using (var client = new WebClient())
            {
                var file = "opus.dll";
                if (!File.Exists(file)) client.DownloadFile("https://discord.foxbot.me/binaries/win64/opus.dll", file);
            }
        }

        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
        }
    }
}
