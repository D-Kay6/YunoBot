using DalFactory;
using Discord.WebSocket;
using IDal.Interfaces.Database;
using System.Threading.Tasks;
using Logic.Extensions;

namespace Logic.Handlers
{
    public class DatabaseHandler
    {
        private DiscordSocketClient _client;
        private IServerSettings _settings;
        private IAutoChannel _autoChannel;
        private IAutoRole _autoRole;

        public async Task Initialize(DiscordSocketClient client)
        {
            this._client = client;
            this._settings = DatabaseFactory.GenerateServerSettings();
            this._autoChannel = DatabaseFactory.GenerateAutoChannel();
            this._autoRole = DatabaseFactory.GenerateAutoRole();

            this._client.Ready += OnReady;
            this._client.JoinedGuild += OnGuildJoined;
            this._client.LeftGuild += OnGuildLeft;
            this._client.GuildUpdated += OnGuildUpdated;
        }

        private async Task OnGuildJoined(SocketGuild guild)
        {
            _settings.RegisterServer(guild.Id, guild.Name);
        }

        private async Task OnGuildLeft(SocketGuild guild)
        {
            _settings.DeleteServer(guild.Id);
        }

        private async Task OnGuildUpdated(SocketGuild oldGuild, SocketGuild newGuild)
        {
            if (oldGuild.Name.Equals(newGuild.Name)) return;
            _settings.UpdateServer(newGuild.Id, newGuild.Name);
        }

        private async Task OnReady()
        {
            await UpdateServers();
        }

        private async Task UpdateServers()
        {
            _client.Guilds.Foreach(g => _settings.UpdateServer(g.Id, g.Name));
        }
    }
}
