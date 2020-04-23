using Discord.WebSocket;
using Logic.Services;
using System;
using System.Threading.Tasks;

namespace Logic.Handlers
{
    public class LoggingHandler : BaseHandler
    {
        private readonly LogsService _logs;

        public LoggingHandler(DiscordSocketClient client, LogsService logs) : base(client)
        {
            _logs = logs;
        }

        public override Task Initialize()
        {
            Client.Connected += ClientOnConnected;
            Client.Disconnected += ClientOnDisconnected;
            Client.LatencyUpdated += ClientOnLatencyUpdated;
            Client.Ready += ClientOnReady;

            Client.UserVoiceStateUpdated += OnVoiceStateUpdate;


            /*
            Client.ChannelCreated
            Client.ChannelDestroyed
            Client.ChannelUpdated
            Client.CurrentUserUpdated
            Client.GuildAvailable
            Client.GuildMemberUpdated
            Client.GuildUnavailable
            Client.GuildMembersDownloaded
            Client.GuildUpdated
            Client.MessageUpdated
            Client.MessagesBulkDeleted
            Client.ReactionAdded
            Client.ReactionRemoved
            Client.ReactionsCleared
            Client.RecipientAdded
            Client.RecipientRemoved
            Client.RoleCreated
            Client.RoleDeleted
            Client.RoleUpdated
            Client.UserBanned
            Client.UserIsTyping
            Client.UserJoined
            Client.UserLeft
            Client.UserUnbanned
            Client.UserUpdated
            Client.VoiceServerUpdated
            Client.JoinedGuild
            Client.LeftGuild
            Client.Log
            Client.LoggedIn
            Client.LoggedOut
            Client.MessageDeleted
            Client.MessageReceived
            */

            return Task.CompletedTask;
        }

        private async Task ClientOnConnected()
        {
            await _logs.Write("Client", "Connected.");
        }

        private async Task ClientOnDisconnected(Exception arg)
        {
            await _logs.Write("Client", $"Disconnected. {arg.Message}");
        }

        private async Task ClientOnLatencyUpdated(int arg1, int arg2)
        {
            await _logs.Write("Client", $"Latency updated from {arg1} to {arg2}.");
        }

        private async Task ClientOnReady()
        {
            await _logs.Write("Client", "Client is ready.");
        }

        private async Task OnVoiceStateUpdate(SocketUser arg1, SocketVoiceState arg2, SocketVoiceState arg3)
        {
            if (arg2.VoiceChannel == arg3.VoiceChannel) return;
            switch (arg2.VoiceChannel)
            {
                case null when arg3.VoiceChannel == null:
                    await _logs.Write("Client", $"VoiceStateUpdate for {arg1.Username} had no from not to channel.");
                    break;
                case null:
                    await _logs.Write("Client", $"{arg1.Username} joined channel {arg3.VoiceChannel.Name}.");
                    break;
                default:
                {
                    if (arg3.VoiceChannel == null) await _logs.Write("Client", $"{arg1.Username} Left channel {arg2.VoiceChannel.Name}.");
                    else await _logs.Write("Client", $"{arg1.Username} moved from {arg2.VoiceChannel.Name} to {arg3.VoiceChannel.Name}.");
                    break;
                }
            }
        }
    }
}