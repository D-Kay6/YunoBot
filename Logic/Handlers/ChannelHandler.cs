using DalFactory;
using Discord;
using Discord.WebSocket;
using IDal.Interfaces.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Net;
using Logic.Extensions;
using Logic.Services.Music;

namespace Logic.Handlers
{
    public class ChannelHandler
    {
        private HashSet<ulong> _channels;
        private DiscordSocketClient _client;
        private IServiceProvider _services;
        private IAutoChannel _autoChannel;

        public async Task Initialize(DiscordSocketClient client, IServiceProvider services)
        {
            _channels = new HashSet<ulong>();
            _client = client;
            _services = services;
            _autoChannel = DatabaseFactory.GenerateAutoChannel();
            _client.UserVoiceStateUpdated += HandleChannelAsync;
        }

        private async Task HandleChannelAsync(SocketUser user, SocketVoiceState state1, SocketVoiceState state2)
        {
            LeaveChannel(state1.VoiceChannel, (SocketGuildUser) user);
            JoinChannel(state2.VoiceChannel, (SocketGuildUser) user);
        }

        private async Task LeaveChannel(SocketVoiceChannel channel, SocketGuildUser user)
        {
            if (channel == null) return;
            var lang = new Localization.Localization(channel.Guild.Id);
            try
            {
                if (channel.Users.Count > 0)
                {
                    if (channel.Users.Count != 1 || !channel.Users.First().Id.Equals(_client.CurrentUser.Id)) return;
                    var audioService = (AudioService)_services.GetService(typeof(AudioService));
                    audioService.BeforeExecute(channel.Guild.Id);
                    await audioService.TextChannel.SendMessageAsync(lang.GetMessage("Channel musicplayer stopped"));
                    await audioService.Stop();
                    return;
                }

                while (_channels.Contains(channel.Id)) await Task.Delay(100);
                
                if (!_autoChannel.IsGeneratedChannel(channel.Guild.Id, channel.Id)) return;
                LogsHandler.Instance.Log("Channels", channel.Guild, $"{user.Nickname()} left channel '{channel.Name}'.");
                if (channel.Guild.GetChannel(channel.Id) == null) return;
                await channel.DeleteAsync();
                _autoChannel.RemoveGeneratedChannel(channel.Guild.Id, channel.Id);
            }
            catch (Exception e)
            {
                var info = channel == null ? "Null channel" : $"({channel.Guild.Id}) {channel.Id}, {channel.Name}";
                LogsHandler.Instance.Log("Crashes", $"LeaveChannel crashed. {info}. Stacktrace: {e}");
            }
        }

        private async Task JoinChannel(SocketVoiceChannel channel, SocketGuildUser user)
        {
            if (channel == null || user == null) return;
            var lang = new Localization.Localization(channel.Guild.Id);
            try
            {
                var channelData = _autoChannel.GetData(channel.Guild.Id);
                if (channel.Name.StartsWith(channelData.PermaPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    var channelId = await DuplicateChannel(channel, user, channelData.PermaName);
                    _channels.Remove(channelId);
                }
                else if (channel.Name.StartsWith(channelData.AutoPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    var channelId = await DuplicateChannel(channel, user, channelData.AutoName);
                    _autoChannel.AddGeneratedChannel(channel.Guild.Id, channelId);
                    _channels.Remove(channelId);
                }
            }
            catch (HttpException httpException)
            {
                if (!httpException.Message.Contains("error 50013: Missing Permissions"))
                {
                    LogsHandler.Instance.Log("Crashes", $"JoinChannel crashed. ({channel.Guild.Id}) {channel.Id}, {channel.Name}. Stacktrace: {httpException}");
                    return;
                }

                var pmChannel = await channel.Guild.Owner.GetOrCreateDMChannelAsync();
                await pmChannel.SendMessageAsync(lang.GetMessage("Channel no permission", channel.Guild.Name, channel.Name));
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
            var newChannel = await channel.Guild.CreateVoiceChannelAsync(string.Format(name, user.Nickname().ToPossessive()), p =>
            {
                p.Bitrate = channel.Bitrate;
                p.CategoryId = channel.CategoryId;
                p.UserLimit = channel.UserLimit;
            });
            _channels.Add(newChannel.Id);

            await user.ModifyAsync(u => u.Channel = newChannel);
            foreach (var p in channel.PermissionOverwrites) await newChannel.AddPermissionOverwriteAsync(channel.Guild.GetRole(p.TargetId), p.Permissions);
            await newChannel.AddPermissionOverwriteAsync(user, new OverwritePermissions(PermValue.Inherit, PermValue.Allow));
            return newChannel.Id;
        }
    }
}