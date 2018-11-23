using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Yuno.Data.Core.Interfaces;
using Yuno.Data.Factory;
using Yuno.Main.AutoChannel;
using Yuno.Main.Commands;
using Yuno.Main.Core;
using Yuno.Main.Music;
using Yuno.Main.Music.YouTube;
using Yuno.Main.Restart;

namespace Yuno.Main
{
    public class Bot : IBot
    {
        private IConfig _config;
        private ISerializer _persistence;

        private DiscordSocketClient _client;

        private CommandHandler _commandHandler;
        private ChannelHandler _channelHandler;

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
        }

        /// <summary>
        /// Asynchronous start of the connection.
        /// </summary>
        public async Task Start()
        {
            while (RestartHandler.KeepAlive)
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
                await _client.SetActivityAsync(new Game("with her Yuki Diary"));
                await _commandHandler.Initialize(_client, _services);
                await _channelHandler.Initialize(_client, _services);
                await RestartHandler.AwaitRestart();
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

        /// <summary>
        /// Print a message to the console.
        /// </summary>
        /// <param name="msg">The message to display</param>
        private async Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
        }
    }
}
