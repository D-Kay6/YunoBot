using DalFactory;
using Discord.WebSocket;
using Entity.RavenDB;
using Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Handlers
{
    public class DatabaseHandler : BaseHandler
    {
        private readonly ServerService _server;
        private readonly UserService _user;

        private bool _isBusy;

        public DatabaseHandler(DiscordShardedClient client, LogsService logs, ServerService server, UserService user) : base(client, logs)
        {
            _server = server;
            _user = user;
        }

        public override Task Initialize()
        {
            base.Initialize();
            Client.UserUpdated += OnUserUpdated;
            Client.JoinedGuild += OnGuildJoined;
            Client.LeftGuild += OnGuildLeft;
            Client.GuildUpdated += OnGuildUpdated;
            return Task.CompletedTask;
        }

        protected override async Task Ready(DiscordSocketClient client)
        {
            await base.Ready(client);

            //await UpdateRaven();
#if RELEASE
            //await UpdateServers();
#endif
        }

        private async Task OnUserUpdated(SocketUser oldState, SocketUser newState)
        {
            if (_isBusy) return;
            if (oldState.Username.Equals(newState.Username)) return;
            await _user.Update(newState);
        }

        private async Task OnGuildJoined(SocketGuild guild)
        {
            if (_isBusy) return;
            await _server.Update(guild);
        }

        private async Task OnGuildLeft(SocketGuild guild)
        {
            if (_isBusy) return;
            await _server.Leave(guild);
        }

        private async Task OnGuildUpdated(SocketGuild oldGuild, SocketGuild newGuild)
        {
            if (_isBusy) return;
            if (oldGuild.Name.Equals(newGuild.Name)) return;
            await _server.Update(newGuild);
        }

        private async Task UpdateRaven()
        {
            Console.WriteLine("Transferring database...");

            var dbServer = RavenDBFactory.GenerateServer();
            var dbChannel = RavenDBFactory.GenerateChannel();
            var dbRole = RavenDBFactory.GenerateRole();

            foreach (var guild in Client.Guilds)
            {
                //while (string.IsNullOrEmpty(guild.Name))
                //{
                //    Console.WriteLine("Loading guild data hasn't finished yet. Waiting 1 second...");
                //    await Task.Delay(1000);
                //}

                //var server = await _server.Update(guild);

                //var channelSettings = new ChannelAutomation
                //{
                //    AutoChannel = new AutoChannel
                //    {
                //        Enabled = server.AutoChannel.Enabled,
                //        Prefix = server.AutoChannel.Prefix,
                //        Name = server.AutoChannel.Name,
                //        Channels = new List<ulong>(server.AutoChannel.Channels.Select(x => x.ChannelId))
                //    },
                //    PermaChannel = new PermaChannel
                //    {
                //        Enabled = server.PermaChannel.Enabled,
                //        Prefix = server.PermaChannel.Prefix,
                //        Name = server.PermaChannel.Name
                //    }
                //};
                //await dbChannel.Add(channelSettings);

                //var roleSettings = new RoleAutomation
                //{
                //    AutoRole = new AutoRole
                //    {
                //        Enabled = server.AutoRole.Enabled,
                //        Prefix = server.AutoRole.Prefix
                //    },
                //    PermaRole = new PermaRole
                //    {
                //        Enabled = server.PermaRole.Enabled,
                //        Prefix = server.PermaRole.Prefix
                //    },
                //    IgnoredUsers = new List<ulong>(server.IgnoredUsers.Select(x => x.UserId))
                //};
                //await dbRole.Add(roleSettings);

                //var rServer = new Server
                //{
                //    ServerId = server.Id,
                //    Name = server.Name,
                //    ChannelAutomation = channelSettings.Id,
                //    RoleAutomation = roleSettings.Id,
                //    LanguageSetting = new LanguageSetting
                //    {
                //        Language = server.LanguageSetting.Language
                //    },
                //    CommandSetting = new CommandSetting
                //    {
                //        Prefix = server.CommandSetting.Prefix,
                //        CustomResponses = server.CustomCommands.ToDictionary(x => x.Command, x => x.Response)
                //    },
                //    WelcomeMessage = new WelcomeMessage
                //    {
                //        ChannelId = server.WelcomeMessage.ChannelId,
                //        Message = server.WelcomeMessage.Message,
                //        UseImage = server.WelcomeMessage.UseImage
                //    }
                //};
                //await dbServer.Add(rServer);
            }

            Console.WriteLine("Done transferring database.");
        }

        private async Task UpdateServers()
        {
            Console.WriteLine("Updating database...");
            _isBusy = true;

            Console.WriteLine("Updating servers...");
            await _server.Update();

            //Console.WriteLine("Updating users...");
            //var users = await _user.GetUsers();
            //foreach (var user in users.Select(dbUser => Client.GetUser(dbUser.Id)).Where(user => user != null))
            //    await _user.UpdateUser(user.Id, user.Username);

            _isBusy = false;
            Console.WriteLine("Done updating database.");
        }
    }
}