using Discord;
using Discord.WebSocket;
using Logic.Exceptions;
using Logic.Models.Chat;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class ChatService
    {
        private readonly ConfigurationService _configuration;
        private readonly Dictionary<ulong, Dictionary<ulong, ChatSession>> _sessions;

        public readonly TimeSpan MaxIdleTime;

        public ChatService(ConfigurationService configuration)
        {
            _configuration = configuration;
            _sessions = new Dictionary<ulong, Dictionary<ulong, ChatSession>>();

            MaxIdleTime = TimeSpan.FromMinutes(1);
        }

        public async Task<ChatSession> CreateSession(IGuildUser user, IChannel channel)
        {
            if (!_sessions.TryGetValue(user.GuildId, out var guildSessions))
            {
                guildSessions = new Dictionary<ulong, ChatSession>();
                _sessions.Add(user.GuildId, guildSessions);
            }

            if (guildSessions.TryGetValue(user.Id, out var session))
            {
                if (session.LastInteraction < DateTime.Now - MaxIdleTime)
                {
                    session.End();
                    guildSessions.Remove(user.Id);
                }
                else
                {
                    throw new SessionExistsException($"User {user.Id} already has a session on this server.");
                }
            }

            var apiToken = await _configuration.GetChatBotApi();
            session = new ChatSession(apiToken, user, channel);
            guildSessions.Add(user.Id, session);
            return session;
        }

        public Task EndSession(IGuildUser user)
        {
            if (!_sessions.TryGetValue(user.GuildId, out var guildSessions))
            {
                throw new InvalidSessionException($"User {user.Id} tried to end a session in a server that has no sessions.");
            }

            if (!guildSessions.TryGetValue(user.Id, out var session))
            {
                throw new InvalidSessionException($"User {user.Id} tried to end a session that does not exist.");
            }

            session.End();
            return Task.CompletedTask;
        }
    }
}