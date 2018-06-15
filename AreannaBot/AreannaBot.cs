using Discord;
using Discord.WebSocket;
using Factory;
using ILogic.Interfaces;
using System;
using System.Threading.Tasks;

namespace AreannaBot
{
    internal class AreannaBot
    {
        private DiscordSocketClient _client;
        private IHandler _commandHandler;
        private IHandler _channelHandler;
        private IConfig _config;

        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        public AreannaBot()
        {
            _config = ConfigFactory.GenerateConfig();
            _commandHandler = HandlerFactory.GenerateCommandHandler();
            _channelHandler = HandlerFactory.GenerateChannelHandler();
        }

        /// <summary>
        /// Asynchronous start of the connection.
        /// </summary>
        public async Task StartAsync()
        {
            if (string.IsNullOrWhiteSpace(_config.GetConfig().Token)) return;
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });
            _client.Log += Log;
            await _client.LoginAsync(TokenType.Bot, _config.GetConfig().Token);
            await _client.StartAsync();
            await _commandHandler.Initialize(_client, _config);
            await _channelHandler.Initialize(_client, _config);
            await Task.Delay(-1); // Stop application from closing.
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
