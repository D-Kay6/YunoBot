using CleverbotApi;
using Discord;
using System;
using System.Threading.Tasks;

namespace Logic.Models.Chat
{
    public class ChatSession
    {
        public ulong Guild { get; }
        public ulong Channel { get; }
        public ulong User { get; }

        public bool HasEnded { get; private set; }
        public DateTime LastInteraction { get; private set; }

        private readonly CleverbotSession Session;
        private CleverbotResponse _response;

        public ChatSession(string apiKey, IGuildUser user, IChannel channel)
        {
            Guild = user.Guild.Id;
            Channel = channel.Id;
            User = user.Id;

            Session = new CleverbotSession(apiKey);
        }

        public async Task<string> GetResponse(string message)
        {
            LastInteraction = DateTime.Now;
            _response = await (_response?.RespondAsync(message) ?? Session.GetResponseAsync(message));
            return _response.Response;
        }

        public void End()
        {
            HasEnded = true;
        }
    }
}