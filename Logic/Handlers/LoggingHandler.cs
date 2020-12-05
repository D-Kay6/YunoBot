using Discord.WebSocket;
using Logic.Services;
using System;
using System.Threading.Tasks;

namespace Logic.Handlers
{
    public class LoggingHandler : BaseHandler
    {
        public LoggingHandler(DiscordShardedClient client, LogsService logs) : base(client, logs) { }

        public override Task Initialize()
        {
            base.Initialize();
            Client.ShardConnected += ClientOnConnected;
            Client.ShardDisconnected += ClientOnDisconnected;
            Client.ShardLatencyUpdated += ClientOnLatencyUpdated;

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

        protected override async Task Ready(DiscordSocketClient client)
        {
            await base.Ready(client);
            await Logs.Write("Client", "Client is ready.");
        }

        private async Task ClientOnConnected(DiscordSocketClient client)
        {
            await Logs.Write("Client", "Connected.");
        }

        private async Task ClientOnDisconnected(Exception arg, DiscordSocketClient client)
        {
            await Logs.Write("Client", $"Disconnected.", arg);
        }

        private async Task ClientOnLatencyUpdated(int arg1, int arg2, DiscordSocketClient client)
        {
            await Logs.Write("Client", $"Latency updated from {arg1} to {arg2}.");
        }

        private async Task OnVoiceStateUpdate(SocketUser arg1, SocketVoiceState arg2, SocketVoiceState arg3)
        {
            if (arg2.VoiceChannel == arg3.VoiceChannel) return;
            switch (arg2.VoiceChannel)
            {
                case null when arg3.VoiceChannel == null:
                    await Logs.Write("Client", $"VoiceStateUpdate for {arg1.Username} had no from not to channel.");
                    break;
                case null:
                    await Logs.Write("Client", $"{arg1.Username} joined channel {arg3.VoiceChannel.Name}.");
                    break;
                default:
                {
                    if (arg3.VoiceChannel == null)
                        await Logs.Write("Client", $"{arg1.Username} Left channel {arg2.VoiceChannel.Name}.");
                    else
                        await Logs.Write("Client",
                            $"{arg1.Username} moved from {arg2.VoiceChannel.Name} to {arg3.VoiceChannel.Name}.");
                    break;
                }
            }
        }
    }
}