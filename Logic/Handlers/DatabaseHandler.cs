namespace Logic.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DalFactory;
    using Discord.WebSocket;
    using Entity.RavenDB;
    using IDal.Database;

    public class DatabaseHandler : BaseHandler
    {
        private readonly IDbServer _server;
        private readonly IDbUser _user;

        private bool _isBusy;

        public DatabaseHandler(DiscordSocketClient client, IDbServer server, IDbUser user) : base(client)
        {
            _server = server;
            _user = user;
        }

        public override Task Initialize()
        {
            Client.Ready += OnReady;
            Client.UserUpdated += OnUserUpdated;
            Client.JoinedGuild += OnGuildJoined;
            Client.LeftGuild += OnGuildLeft;
            Client.GuildUpdated += OnGuildUpdated;
            return Task.CompletedTask;
        }

        private async Task OnReady()
        {
            //await UpdateRaven();
#if RELEASE
            //await UpdateServers();
#endif
        }

        private async Task OnUserUpdated(SocketUser oldState, SocketUser newState)
        {
            if (_isBusy) return;
            if (oldState.Username.Equals(newState.Username)) return;
            await _user.UpdateUser(newState.Id, newState.Username);
        }

        private async Task OnGuildJoined(SocketGuild guild)
        {
            if (_isBusy) return;
            await _server.AddServer(guild.Id, guild.Name);
        }

        private async Task OnGuildLeft(SocketGuild guild)
        {
            if (_isBusy) return;
            await _server.DeleteServer(guild.Id);
        }

        private async Task OnGuildUpdated(SocketGuild oldGuild, SocketGuild newGuild)
        {
            if (_isBusy) return;
            if (oldGuild.Name.Equals(newGuild.Name)) return;
            await _server.UpdateServer(newGuild.Id, newGuild.Name);
        }

        private async Task UpdateRaven()
        {
            Console.WriteLine("Transferring database...");

            var dbServer = RavenDBFactory.GenerateServer();
            var dbChannel = RavenDBFactory.GenerateChannel();
            var dbRole = RavenDBFactory.GenerateRole();

            foreach (var guild in Client.Guilds)
            {
                while (string.IsNullOrEmpty(guild.Name))
                {
                    Console.WriteLine("Loading guild data hasn't finished yet. Waiting 1 second...");
                    await Task.Delay(1000);
                }

                var server = await _server.GetServer(guild.Id);

                var channelSettings = new ChannelAutomation
                {
                    AutoChannel = new AutoChannel
                    {
                        Enabled = server.AutoChannel.Enabled,
                        Prefix = server.AutoChannel.Prefix,
                        Name = server.AutoChannel.Name,
                        Channels = new List<ulong>(server.AutoChannel.Channels.Select(x => x.ChannelId))
                    },
                    PermaChannel = new PermaChannel
                    {
                        Enabled = server.PermaChannel.Enabled,
                        Prefix = server.PermaChannel.Prefix,
                        Name = server.PermaChannel.Name
                    }
                };
                await dbChannel.Add(channelSettings);

                var roleSettings = new RoleAutomation
                {
                    AutoRole = new AutoRole
                    {
                        Enabled = server.AutoRole.Enabled,
                        Prefix = server.AutoRole.Prefix
                    },
                    PermaRole = new PermaRole
                    {
                        Enabled = server.PermaRole.Enabled,
                        Prefix = server.PermaRole.Prefix
                    },
                    IgnoredUsers = new List<ulong>(server.IgnoredUsers.Select(x => x.UserId))
                };
                await dbRole.Add(roleSettings);

                var rServer = new Server
                {
                    ServerId = server.Id,
                    Name = server.Name,
                    ChannelAutomation = channelSettings.Id,
                    RoleAutomation = roleSettings.Id,
                    LanguageSetting = new LanguageSetting
                    {
                        Language = server.LanguageSetting.Language
                    },
                    CommandSetting = new CommandSetting
                    {
                        Prefix = server.CommandSetting.Prefix,
                        CustomResponses = server.CustomCommands.ToDictionary(x => x.Command, x => x.Response)
                    },
                    WelcomeMessage = new WelcomeMessage
                    {
                        ChannelId = server.WelcomeMessage.ChannelId,
                        Message = server.WelcomeMessage.Message,
                        UseImage = server.WelcomeMessage.UseImage
                    }
                };
                await dbServer.Add(rServer);
            }

            Console.WriteLine("Done transferring database.");
        }

        private async Task UpdateServers()
        {
            Console.WriteLine("Updating database...");
            _isBusy = true;

            foreach (var guild in Client.Guilds)
            {
                while (string.IsNullOrEmpty(guild.Name))
                {
                    Console.WriteLine("Loading guild data hasn't finished yet. Waiting 1 second...");
                    await Task.Delay(1000);
                }

                await _server.UpdateServer(guild.Id, guild.Name);
            }

            var users = await _user.GetUsers();
            foreach (var user in users.Select(dbUser => Client.GetUser(dbUser.Id)).Where(user => user != null))
                await _user.UpdateUser(user.Id, user.Username);

            _isBusy = false;
            Console.WriteLine("Done updating database.");
        }
    }
}