using Discord;
using Discord.Net;
using Discord.WebSocket;
using Logic.Extensions;
using Logic.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IDal.Interfaces.Database;
using IChannel = IDal.Interfaces.Database.IChannel;

namespace Logic.Handlers
{
    public class ChannelHandler : BaseHandler
    {
        private ILanguage _language;
        private IChannel _channel;
        private AudioService _audioService;

        private HashSet<ulong> _channels;
        
        public ChannelHandler(DiscordSocketClient client, ILanguage language, IChannel channel, AudioService audioService) : base(client)
        {
            _language = language;
            _channel = channel;
            _audioService = audioService;
            _channels = new HashSet<ulong>();
        }

        public override async Task Initialize()
        {
            Client.UserVoiceStateUpdated += HandleChannelAsync;
        }

        private async Task HandleChannelAsync(SocketUser user, SocketVoiceState state1, SocketVoiceState state2)
        {
            LeaveChannel(state1.VoiceChannel, (SocketGuildUser) user);
            JoinChannel(state2.VoiceChannel, (SocketGuildUser) user);
        }

        private async Task LeaveChannel(SocketVoiceChannel channel, SocketGuildUser user)
        {
            if (channel == null) return;
            var lang = new Localization.Localization(_language.GetLanguage(channel.Guild.Id));
            try
            {
                if (channel.Users.Count > 0)
                {
                    if (channel.Users.Count != 1 || !channel.Users.First().Id.Equals(Client.CurrentUser.Id)) return;
                    _audioService.BeforeExecute(channel.Guild);
                    await _audioService.TextChannel.SendMessageAsync(lang.GetMessage("Channel musicplayer stopped"));
                    await _audioService.Stop();
                    return;
                }

                while (_channels.Contains(channel.Id)) await Task.Delay(100);
                
                if (!_channel.IsGeneratedChannel(channel.Guild.Id, channel.Id)) return;
                LogService.Instance.Log("Channels", channel.Guild, $"{user.Nickname()} left channel '{channel.Name}'.");
                if (channel.Guild.GetChannel(channel.Id) == null) return;
                await channel.DeleteAsync();
                _channel.RemoveGeneratedChannel(channel.Guild.Id, channel.Id);
            }
            catch (Exception e)
            {
                var info = channel == null ? "Null channel" : $"({channel.Guild.Id}) {channel.Id}, {channel.Name}";
                LogService.Instance.Log("Crashes", $"LeaveChannel crashed. {info}. Stacktrace: {e}");
            }
        }

        private async Task JoinChannel(SocketVoiceChannel channel, SocketGuildUser user)
        {
            if (channel == null || user == null) return;
            var lang = new Localization.Localization(_language.GetLanguage(channel.Guild.Id));
            try
            {
                var auto = _channel.GetAutoChannel(channel.Guild.Id);
                if (channel.Name.StartsWith(auto.Prefix, StringComparison.OrdinalIgnoreCase))
                {
                    var channelId = await DuplicateChannel(channel, user, auto.Name);
                    _channel.AddGeneratedChannel(channel.Guild.Id, channelId);
                    _channels.Remove(channelId);
                    return;
                }

                var perma = _channel.GetPermaChannel(channel.Guild.Id);
                if (channel.Name.StartsWith(perma.Prefix, StringComparison.OrdinalIgnoreCase))
                {
                    var channelId = await DuplicateChannel(channel, user, perma.Name);
                    _channels.Remove(channelId);
                }
            }
            catch (HttpException httpException)
            {
                if (!httpException.Message.Contains("error 50013: Missing Permissions"))
                {
                    LogService.Instance.Log("Crashes", $"JoinChannel crashed. ({channel.Guild.Id}) {channel.Id}, {channel.Name}. Stacktrace: {httpException}");
                    return;
                }

                var pmChannel = await channel.Guild.Owner.GetOrCreateDMChannelAsync();
                await pmChannel.SendMessageAsync(lang.GetMessage("Channel no permission", channel.Guild.Name, channel.Name));
            }
            catch (Exception e)
            {
                var info = channel == null ? "Null channel" : $"({channel.Guild.Id}) {channel.Id}, {channel.Name}";
                LogService.Instance.Log("Crashes", $"JoinChannel crashed. {info}. Stacktrace: {e}");
            }
        }

        private async Task<ulong> DuplicateChannel(SocketVoiceChannel channel, SocketGuildUser user, string name)
        {
            LogService.Instance.Log("Channels", channel.Guild, $"{user.Username} joined channel '{channel.Name}'.");
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