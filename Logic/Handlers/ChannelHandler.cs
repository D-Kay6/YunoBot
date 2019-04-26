using Discord;
using Discord.WebSocket;
using Logic.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Logic.Services;

namespace Logic.Handlers
{
    public class ChannelHandler
    {
        private HashSet<ulong> _channels;
        private DiscordSocketClient _client;
        private IServiceProvider _services;

        public async Task Initialize(DiscordSocketClient client, IServiceProvider services)
        {
            _channels = new HashSet<ulong>();
            _client = client;
            _services = services;
            _client.UserVoiceStateUpdated += HandleChannelAsync;
        }

        private async Task HandleChannelAsync(SocketUser user, SocketVoiceState state1, SocketVoiceState state2)
        {
            LeaveChannel(state1.VoiceChannel, (SocketGuildUser) user);
            JoinChannel(state2.VoiceChannel, (SocketGuildUser) user);
        }

        private async Task LeaveChannel(SocketVoiceChannel channel, SocketGuildUser user)
        {
            try
            {
                if (channel == null) return;

                if (channel.Users.Count > 0)
                {
                    if (channel.Users.Count != 1 || !channel.Users.First().Id.Equals(_client.CurrentUser.Id)) return;
                    var audioService = (AudioService)_services.GetService(typeof(AudioService));
                    audioService.BeforeExecute(channel.Guild.Id);
                    await audioService.TextChannel.SendMessageAsync("The music player was stopped.\nAll users have left the voice channel.");
                    await audioService.Stop();
                    return;
                }

                while (_channels.Contains(channel.Id)) await Task.Delay(100);

                var autoChannel = AutoChannel.Load(channel.Guild.Id);
                if (!autoChannel.IsControlledChannel(channel.Id)) return;
                var userName = user?.Nickname ?? user?.Username ?? "User";
                LogsHandler.Instance.Log("Channels", channel.Guild, $"{userName} left channel '{channel.Name}'.");
                autoChannel.RemoveChannel(channel.Id);
                autoChannel.Save();
                if (channel.Guild.GetChannel(channel.Id) == null) return;
                await channel.DeleteAsync();
            }
            catch (Exception e)
            {
                var info = channel == null ? "Null channel" : $"({channel.Guild.Id}) {channel.Id}, {channel.Name}";
                LogsHandler.Instance.Log("Crashes", $"LeaveChannel crashed. {info}. Stacktrace: {e}");
            }
        }

        private async Task JoinChannel(SocketVoiceChannel channel, SocketGuildUser user)
        {
            try
            {
                if (channel == null || user == null) return;
                var autoChannel = AutoChannel.Load(channel.Guild.Id);
                if (autoChannel.IsPermaChannel(channel))
                {
                    var channelId = await DuplicateChannel(channel, user, autoChannel.PermaName);
                    _channels.Remove(channelId);
                }
                else if (autoChannel.IsAutoChannel(channel))
                {
                    var channelId = await DuplicateChannel(channel, user, autoChannel.AutoName);
                    autoChannel.AddChannel(channelId);
                    autoChannel.Save();
                    _channels.Remove(channelId);
                }
            }
            catch (Exception e)
            {
                var info = channel == null ? "Null channel" : $"({channel.Guild.Id}) {channel.Id}, {channel.Name}";
                LogsHandler.Instance.Log("Crashes", $"JoinChannel crashed. {info}. Stacktrace: {e}");
            }
        }

        private async Task<ulong> DuplicateChannel(SocketVoiceChannel channel, SocketGuildUser user, string name)
        {
            LogsHandler.Instance.Log("Channels", channel.Guild, $"{user.Username} joined channel '{channel.Name}'.");
            var userName = user.Nickname ?? user.Username;
            userName += userName.EndsWith("s", StringComparison.CurrentCultureIgnoreCase) ? "'" : "'s";
            var newChannel = await channel.Guild.CreateVoiceChannelAsync(string.Format(name, userName), p =>
            {
                p.Bitrate = channel.Bitrate;
                p.CategoryId = channel.CategoryId;
                p.UserLimit = channel.UserLimit;
            });
            _channels.Add(newChannel.Id);

            await user.ModifyAsync(u => u.Channel = newChannel);
            foreach (var p in channel.PermissionOverwrites)
                await newChannel.AddPermissionOverwriteAsync(channel.Guild.GetRole(p.TargetId), p.Permissions);
            //var tasks = channel.PermissionOverwrites.Select(o => newChannel.AddPermissionOverwriteAsync(channel.Guild.GetRole(o.TargetId), o.Permissions));
            //await Task.WhenAll(tasks);
            await newChannel.AddPermissionOverwriteAsync(user, new OverwritePermissions(PermValue.Inherit, PermValue.Allow));
            return newChannel.Id;
        }
    }
}