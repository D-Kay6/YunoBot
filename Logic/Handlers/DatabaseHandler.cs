using System;
using DalFactory;
using Discord.WebSocket;
using IDal.Interfaces.Database;
using System.Threading.Tasks;
using Logic.Extensions;

namespace Logic.Handlers
{
    public class DatabaseHandler : BaseHandler
    {
        private readonly IServerSettings _settings;
        private readonly IAutoChannel _autoChannel;
        private readonly IAutoRole _autoRole;

        public DatabaseHandler(DiscordSocketClient client, IServiceProvider serviceProvider) : base(client, serviceProvider)
        {
            this.Client = client;
            this._settings = DatabaseFactory.GenerateServerSettings();
            this._autoChannel = DatabaseFactory.GenerateAutoChannel();
            this._autoRole = DatabaseFactory.GenerateAutoRole();
        }

        public override async Task Initialize()
        {
            this.Client.JoinedGuild += OnGuildJoined;
            this.Client.LeftGuild += OnGuildLeft;
            this.Client.GuildUpdated += OnGuildUpdated;

            await UpdateServers();
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

        private async Task UpdateServers()
        {
            Client.Guilds.Foreach(g => _settings.UpdateServer(g.Id, g.Name));
        }
    }
}
