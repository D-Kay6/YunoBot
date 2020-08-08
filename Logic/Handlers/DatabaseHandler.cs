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
        private readonly RoleService _role;
        private readonly UserService _user;

        private bool _isBusy;

        public DatabaseHandler(DiscordShardedClient client, LogsService logs, ServerService server, RoleService role, UserService user) : base(client, logs)
        {
            _server = server;
            _role = role;
            _user = user;
        }

        public override Task Initialize()
        {
            base.Initialize();
            Client.UserUpdated += OnUserUpdated;
            Client.RoleUpdated += OnRoleUpdated;
            Client.GuildUpdated += OnGuildUpdated;
            Client.JoinedGuild += OnGuildJoined;
            Client.LeftGuild += OnGuildLeft;
            return Task.CompletedTask;
        }

        protected override async Task Ready(DiscordSocketClient client)
        {
            await base.Ready(client);

#if RELEASE
            //await UpdateServers();
#endif
        }

        private async Task OnRoleUpdated(SocketRole oldState, SocketRole newState)
        {
            if (_isBusy) return;
            if (oldState.Name.Equals(newState.Name)) return;
            await _role.Update(newState);
        }

        private async Task OnUserUpdated(SocketUser oldState, SocketUser newState)
        {
            if (_isBusy) return;
            if (oldState.Username.Equals(newState.Username)) return;
            await _user.Update(newState);
        }

        private async Task OnGuildUpdated(SocketGuild oldGuild, SocketGuild newGuild)
        {
            if (_isBusy) return;
            if (oldGuild.Name.Equals(newGuild.Name)) return;
            await _server.Update(newGuild);
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