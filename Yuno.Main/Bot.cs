using System;
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
            _channelHandler = new ChannelHandler(_persistence);
        }

        /// <summary>
        /// Asynchronous start of the connection.
        /// </summary>
        public async Task Start()
        {
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
            _services.GetService<SongService>().AudioPlaybackService = _services.GetService<AudioPlaybackService>();

            await _client.LoginAsync(TokenType.Bot, config.Token);
            await _client.StartAsync();
            await _commandHandler.Initialize(_client, _services, config.Prefix);
            await _channelHandler.Initialize(_client);
            await Task.Delay(-1); // Stop application from closing.
        }

        private void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton(new YouTubeDownloadService());
            serviceCollection.AddSingleton(new AudioPlaybackService());
            serviceCollection.AddSingleton(new SongService());
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
