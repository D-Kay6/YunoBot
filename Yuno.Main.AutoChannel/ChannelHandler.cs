using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Yuno.Data.Core.Interfaces;
using Yuno.Logic;
using Yuno.Main.Logging;
using Yuno.Main.Music;

namespace Yuno.Main.AutoChannel
{
    public class ChannelHandler
    {
        private DiscordSocketClient _client;

        public async Task Initialize(DiscordSocketClient client, IServiceProvider services)
        {
            this._client = client;
            _client.UserVoiceStateUpdated += HandleChannelAsync;
        }

        private async Task HandleChannelAsync(SocketUser user, SocketVoiceState state1, SocketVoiceState state2)
        {
            if (state1.VoiceChannel != null) LeaveChannel(state1.VoiceChannel, (SocketGuildUser)user);
            
            if (state2.VoiceChannel != null) JoinChannel(state2.VoiceChannel, (SocketGuildUser)user);
        }

        private async void LeaveChannel(SocketVoiceChannel channel, SocketGuildUser user)
        {
            try
            {
                if (channel == null) return;

                if (channel.Users.Count != 0)
                {
                    if (channel.Users.Count != 1 || !channel.Users.First().Id.Equals(_client.CurrentUser.Id)) return;
                    var songService = SongService.GetSongService(channel.Guild.Id);
                    songService.Stop("All users left the voice channel.");
                    return;
                }
                var autoChannel = Logic.AutoChannel.Load(channel.Guild.Id);
                if (!autoChannel.IsControlledChannel(channel.Id)) return;
                var userName = user?.Nickname ?? user?.Username ?? "User";
                LogsHandler.Instance.Log("Channels", channel.Guild, $"{userName} left channel '{channel.Name}'.");
                autoChannel.RemoveChannel(channel.Id);
                autoChannel.Save();
                await channel.DeleteAsync();
            }
            catch (Exception e)
            {
                var info = channel == null ? "Null channel" : $"({channel.Guild.Id}) {channel.Id}, {channel.Name}";
                LogsHandler.Instance.Log("Crashes", $"LeaveChannel crashed. {info}. Stacktrace: {e}");
            }
        }

        private async void JoinChannel(SocketVoiceChannel channel, SocketGuildUser user)
        {
            if (channel == null || user == null) return;
            var autoChannel = Logic.AutoChannel.Load(channel.Guild.Id);
            if (autoChannel.IsPermaChannel(channel))
            {
                LogsHandler.Instance.Log("Channels", channel.Guild, $"{user.Username} joined channel '{channel.Name}'.");
                var nameEnding = user.Username.EndsWith("s", StringComparison.CurrentCultureIgnoreCase) ? "'" : "'s";
                await DuplicateChannel(channel, user, $"{user.Username}{nameEnding} channel");
            }
            else if (autoChannel.IsAutoChannel(channel))
            {
                LogsHandler.Instance.Log("Channels", channel.Guild, $"{user.Username} joined channel '{channel.Name}'.");
                autoChannel.AddChannel(await DuplicateChannel(channel, user));
                autoChannel.Save();
            }
        }

        private async Task<ulong> DuplicateChannel(SocketVoiceChannel channel, SocketGuildUser user, string name = "--channel")
        {
            var newChannel = await channel.Guild.CreateVoiceChannelAsync(name);
            await newChannel.ModifyAsync(v =>
            {
                v.Bitrate = channel.Bitrate;
                v.CategoryId = channel.CategoryId;
                v.UserLimit = channel.UserLimit;
            });
            await (user.ModifyAsync(a => a.Channel = newChannel));
            var tasks = channel.PermissionOverwrites.Select(o => newChannel.AddPermissionOverwriteAsync(channel.Guild.GetRole(o.TargetId), o.Permissions));
            await Task.WhenAll(tasks);
            await newChannel.AddPermissionOverwriteAsync(user, new OverwritePermissions(PermValue.Inherit, PermValue.Allow));
            return newChannel.Id;
        }
    }
}
