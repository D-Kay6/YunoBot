using Discord.WebSocket;
using Logic.Services;
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

            _isBusy = true;
        }

        public override Task Initialize()
        {
            base.Initialize();
            Client.UserUpdated += OnUserUpdated;
            Client.GuildUpdated += OnGuildUpdated;
            Client.JoinedGuild += OnGuildJoined;
            Client.LeftGuild += OnGuildLeft;
            return Task.CompletedTask;
        }

        protected override async Task Ready(DiscordSocketClient client)
        {
            await base.Ready(client);
            _isBusy = false;
        }

        private async Task OnUserUpdated(SocketUser oldState, SocketUser newState)
        {
            if (_isBusy) return;
            if (string.IsNullOrEmpty(oldState.Username)) return;
            if (oldState.Username.Equals(newState.Username)) return;
            await _user.Update(newState);
        }

        private async Task OnGuildUpdated(SocketGuild oldGuild, SocketGuild newGuild)
        {
            if (_isBusy) return;
            if (string.IsNullOrEmpty(oldGuild.Name)) return;
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
    }
}