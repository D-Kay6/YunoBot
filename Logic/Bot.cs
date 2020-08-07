using DalFactory;
using Discord;
using Discord.WebSocket;
using ILogic;
using Logic.Exceptions;
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
        private readonly DiscordShardedClient _client;

        private readonly HandlerCollection _handlers;

        private readonly IServiceProvider _services;

        public Bot()
        {
            _client = new DiscordShardedClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Verbose,
                ConnectionTimeout = 30000,
#if !DEBUG
                GatewayIntents = GatewayIntents.Guilds |
                                 GatewayIntents.GuildMembers |
                                 GatewayIntents.GuildIntegrations |
                                 GatewayIntents.GuildVoiceStates |
                                 GatewayIntents.GuildMessages |
                                 GatewayIntents.GuildMessageReactions |
                                 GatewayIntents.DirectMessages
#endif
                //TotalShards = 2
            });
            _client.Log += Log;
            _client.ShardReady += OnReady;

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
                try
                {
                    DownloadPrerequisites();
                    var token = await configService.GetToken();

                    await _handlers.Start();
                    await _client.LoginAsync(TokenType.Bot, token);
                    await _client.StartAsync();

                    await restartService.AwaitRestart();
                }
                catch (InvalidTokenException)
                {
                    await logsService.Write("Main", "The bot token in the config file could not be read.");
                    break;
                }
                catch (Exception e)
                {
                    await logsService.Write("Main", $"Fatal exception occured. Restarting bot.", e);
                }
                finally
                {
                    await _handlers.Stop();
                    await _client.StopAsync();
                }

            await _handlers.Finish();
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
            serviceCollection.AddTransient(serviceProvider => DatabaseFactory.GenerateCommandSetting());
            serviceCollection.AddTransient(serviceProvider => DatabaseFactory.GenerateCommandCustom());
            serviceCollection.AddTransient(serviceProvider => DatabaseFactory.GenerateWelcome());
            serviceCollection.AddTransient(serviceProvider => DatabaseFactory.GenerateAutoChannel());
            serviceCollection.AddTransient(serviceProvider => DatabaseFactory.GenerateGeneratedChannel());
            serviceCollection.AddTransient(serviceProvider => DatabaseFactory.GeneratePermaChannel());
            serviceCollection.AddTransient(serviceProvider => DatabaseFactory.GenerateAutoRole());
            serviceCollection.AddTransient(serviceProvider => DatabaseFactory.GeneratePermaRole());
            serviceCollection.AddTransient(serviceProvider => DatabaseFactory.GenerateRoleIgnore());

            serviceCollection.AddTransient(serviceProvider => ConfigFactory.GenerateConfig());
            serviceCollection.AddTransient(serviceProvider => LocalizationFactory.GenerateLocalization());

            serviceCollection.AddSingleton(_client);

            serviceCollection.AddSingleton<LoggingHandler>();
            serviceCollection.AddSingleton<ConfigurationService>();
            serviceCollection.AddSingleton<ServerService>();
            serviceCollection.AddSingleton<UserService>();
            serviceCollection.AddSingleton<CommandService>();
            serviceCollection.AddSingleton<ChannelService>();
            serviceCollection.AddSingleton<RoleService>();
            serviceCollection.AddSingleton<LogsService>();
            serviceCollection.AddSingleton<RestartService>();
            serviceCollection.AddSingleton<WelcomeService>();
            serviceCollection.AddSingleton<MusicService>();

            serviceCollection.AddTransient<LocalizationService>();

            return serviceCollection.BuildServiceProvider();
        }

        private void DownloadPrerequisites()
        {
            using var client = new WebClient();
            var file = "libsodium.dll";
            if (!File.Exists(file))
                client.DownloadFile("https://discord.foxbot.me/binaries/win64/libsodium.dll", file);

            file = "opus.dll";
            if (!File.Exists(file)) client.DownloadFile("https://discord.foxbot.me/binaries/win64/opus.dll", file);
        }

        private async Task OnReady(DiscordSocketClient client)
        {
            var logService = _services.GetService<LogsService>();
            var shardCount = await _client.GetRecommendedShardCountAsync();
            if (shardCount > 1)
                await logService.Write("Main", $"Probably time to think about creating shards. {shardCount}");
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
            return Task.CompletedTask;
        }
    }
}