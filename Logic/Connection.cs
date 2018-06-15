using Discord;
using Discord.WebSocket;
using Logic.Channels;
using Logic.Commands;
using Logic.Configuration.Settings;
using System;
using System.Threading.Tasks;
using DalFactory;
using IConnection = ILogic.Interfaces.IConnection;

namespace Logic
{
    public class Connection : IConnection
    {
        private DiscordSocketClient _client;
        private CommandHandler _commandHandler;
        private ChannelHandler _channelHandler;
        private Config _config;

        /// <summary>
        /// Creates a new instance of this class.
        /// </summary>
        public Connection()
        {
            _commandHandler = new CommandHandler();
            _channelHandler = new ChannelHandler();
            _config = new Config(ConfigDalFactory.GenerateConfigDal());
        }

        /// <summary>
        /// Asynchronous start of the connection.
        /// </summary>
        public async Task Start()
        {
            if (string.IsNullOrWhiteSpace(_config.Token)) return;
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose
            });
            _client.Log += Log;
            await _client.LoginAsync(TokenType.Bot, _config.Token);
            await _client.StartAsync();
            await _commandHandler.Initialize(_client, _config.Prefix);
            await _channelHandler.Initialize(_client);
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
