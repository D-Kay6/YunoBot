using Core.Entity;
using Discord;
using Discord.WebSocket;
using IDal.Database;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class ServerService
    {
        private readonly DiscordSocketClient _client;
        private readonly IDbServer _dbServer;

        public ServerService(DiscordSocketClient client, IDbServer dbServer)
        {
            _client = client;
            _dbServer = dbServer;
        }

        /// <summary>
        ///     Update the data of all connected servers.
        /// </summary>
        public async Task Update()
        {
            foreach (var server in _client.Guilds)
            {
                while (string.IsNullOrEmpty(server.Name))
                    await Task.Delay(1000);

                await Update(server);
            }
        }

        /// <summary>
        ///     Update the details of the server.
        ///     If the server does not exist yet, it is added.
        /// </summary>
        /// <param name="server">The server to update.</param>
        /// <returns>The details of the server.</returns>
        public async Task<Server> Update(IGuild server)
        {
            var settings = await _dbServer.Get(server.Id);
            if (settings == null)
            {
                settings = new Server
                {
                    Id = server.Id,
                    Name = server.Name
                };
                await _dbServer.Add(settings);
                return settings;
            }

            if (settings.Name != server.Name)
            {
                settings.Name = server.Name;
                await _dbServer.Update(settings);
            }

            return settings;
        }

        /// <summary>
        ///     Remove all settings from a server.
        /// </summary>
        /// <param name="server">The server to update.</param>
        public async Task Leave(IGuild server)
        {
            var settings = await _dbServer.Get(server.Id);
            if (settings == null) return;

            await _dbServer.Remove(settings);
        }
    }
}