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

        public DatabaseHandler(DiscordSocketClient client, IDbServer server, IDbUser user) : base(client)
        {
            _server = server;
            _user = user;
        }

        public override async Task Initialize()
        {
            Client.Ready += OnReady;
        }

        private async Task OnReady()
        {
            if (!IsLoaded())
            {
                Client.UserUpdated += OnUserUpdated;
                Client.JoinedGuild += OnGuildJoined;
                Client.LeftGuild += OnGuildLeft;
                Client.GuildUpdated += OnGuildUpdated;
                FinishLoading();
            }

            await UpdateServers().ConfigureAwait(false);
        }

        private async Task OnUserUpdated(SocketUser oldState, SocketUser newState)
        {
            if (oldState.Username.Equals(newState.Username)) return;
            await _user.UpdateUser(newState.Id, newState.Username);
        }

        private async Task OnGuildJoined(SocketGuild guild)
        {
            await _server.AddServer(guild.Id, guild.Name);
        }

        private async Task OnGuildLeft(SocketGuild guild)
        {
            await _server.DeleteServer(guild.Id);
        }

        private async Task OnGuildUpdated(SocketGuild oldGuild, SocketGuild newGuild)
        {
            if (oldGuild.Name.Equals(newGuild.Name)) return;
            await _server.UpdateServer(newGuild.Id, newGuild.Name);
        }

        private async Task UpdateServers()
        {
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
        }
    }
}
