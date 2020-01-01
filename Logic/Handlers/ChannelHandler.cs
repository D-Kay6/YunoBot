using Discord;
using Discord.Net;
using Discord.WebSocket;
using IDal.Database;
using Logic.Extensions;
using Logic.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Handlers
{
    public class ChannelHandler : BaseHandler
    {
        private IDbChannel _channel;
        private IDbLanguage _language;
        private LocalizationService _localization;
        private LogsService _logs;

        private HashSet<ulong> _channels;
        
        public ChannelHandler(DiscordSocketClient client, IDbChannel channel, IDbLanguage language, LocalizationService localization, LogsService logs) : base(client)
        {
            _channel = channel;
            _language = language;
            _localization = localization;
            _logs = logs;
            _channels = new HashSet<ulong>();
        }

        public override async Task Initialize()
        {
            Client.Ready += OnReady;
        }

        private async Task OnReady()
        {
            if (IsLoaded()) return;
            Client.UserVoiceStateUpdated += HandleChannelAsync;
            FinishLoading();
        }

        private async Task HandleChannelAsync(SocketUser user, SocketVoiceState state1, SocketVoiceState state2)
        {
            try
            {
                var guildUser = user as SocketGuildUser;
                LeaveChannel(state1.VoiceChannel, guildUser);
                JoinChannel(state2.VoiceChannel, guildUser);
            }
            catch (Exception e)
            {
                await _logs.Write("Crashes", $"Failed to handle UserVoiceStateUpdated. {e.Message}, {e.StackTrace}");
            }
        }

        private async Task LoadLanguage(ulong serverId)
        {
            await _localization.Load(await _language.GetLanguage(serverId));
        }

        private async Task LeaveChannel(SocketVoiceChannel channel, SocketGuildUser user)
        {
            if (channel == null) return;
            try
            {
                if (channel.Users.Count > 0) return;

                while (_channels.Contains(channel.Id)) await Task.Delay(100);
                
                if (!await _channel.IsGeneratedChannel(channel.Guild.Id, channel.Id)) return;
                await _logs.Write("Channels", channel.Guild, $"{user.Nickname()} left channel '{channel.Name}'.");
                if (channel.Guild.GetChannel(channel.Id) == null) return;
                await channel.DeleteAsync();
                await _channel.RemoveGeneratedChannel(channel.Guild.Id, channel.Id);
            }
            catch (Exception e)
            {
                var info = channel == null ? "Null channel" : $"({channel.Guild.Id}) {channel.Id}, {channel.Name}";
                await _logs.Write("Crashes", $"LeaveChannel crashed. {info}. Stacktrace: {e}");
            }
        }

        private async Task JoinChannel(SocketVoiceChannel channel, SocketGuildUser user)
        {
            if (channel == null || user == null) return;
            try
            {
                var auto = await _channel.GetAutoChannel(channel.Guild.Id);
                if (channel.Name.StartsWith(auto.Prefix, StringComparison.OrdinalIgnoreCase))
                {
                    var channelId = await DuplicateChannel(channel, user, auto.Name);
                    await _channel.AddGeneratedChannel(channel.Guild.Id, channelId);
                    _channels.Remove(channelId);
                    return;
                }

                var perma = await _channel.GetPermaChannel(channel.Guild.Id);
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
                    await _logs.Write("Crashes", $"JoinChannel crashed. ({channel.Guild.Id}) {channel.Id}, {channel.Name}. Stacktrace: {httpException}");
                    return;
                }

                var pmChannel = await channel.Guild.Owner.GetOrCreateDMChannelAsync();
                await LoadLanguage(channel.Guild.Id);
                await pmChannel.SendMessageAsync(_localization.GetMessage("Channel no permission", channel.Guild.Name, channel.Name));
            }
            catch (Exception e)
            {
                var info = channel == null ? "Null channel" : $"({channel.Guild.Id}) {channel.Id}, {channel.Name}";
                await _logs.Write("Crashes", $"JoinChannel crashed. {info}. Stacktrace: {e}");
            }
        }

        private async Task<ulong> DuplicateChannel(SocketVoiceChannel channel, SocketGuildUser user, string name)
        {
            await _logs.Write("Channels", channel.Guild, $"{user.Username} joined channel '{channel.Name}'.");
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