using Discord.WebSocket;
using IDal.Database;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Handlers
{
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
#if RELEASE
            await UpdateServers();
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
            {
                await _user.UpdateUser(user.Id, user.Username);
            }

            _isBusy = false;
            Console.WriteLine("Done updating database.");
        }
    }
}
