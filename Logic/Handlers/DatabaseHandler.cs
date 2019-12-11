using DalFactory;
using Discord.WebSocket;
using Entity;
using IDal.Interfaces.Database;
using Logic.Extensions;
using System.Threading.Tasks;

namespace Logic.Handlers
{
    public class DatabaseHandler : BaseHandler
    {
        private readonly IServer _server;

        public DatabaseHandler(DiscordSocketClient client, IServer server) : base(client)
        {
            _server = server;
        }

        public override async Task Initialize()
        {
            Client.Ready += OnReady;
            Client.JoinedGuild += OnGuildJoined;
            Client.LeftGuild += OnGuildLeft;
            Client.GuildUpdated += OnGuildUpdated;
        }

        private async Task OnReady()
        {
            await UpdateServers();
        }

        private async Task OnGuildJoined(SocketGuild guild)
        {
            _server.AddServer(guild.Id, guild.Name);
        }

        private async Task OnGuildLeft(SocketGuild guild)
        {
            _server.DeleteServer(guild.Id);
        }

        private async Task OnGuildUpdated(SocketGuild oldGuild, SocketGuild newGuild)
        {
            if (oldGuild.Name.Equals(newGuild.Name)) return;
            _server.UpdateServer(newGuild.Id, newGuild.Name);
        }

        private async Task UpdateServers()
        {
            Client.Guilds.Foreach(g => _server.UpdateServer(g.Id, g.Name));
        }
    }
}
