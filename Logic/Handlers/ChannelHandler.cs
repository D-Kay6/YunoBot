namespace Logic.Handlers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Net;
    using Discord.Rest;
    using Discord.WebSocket;
    using Extensions;
    using IDal.Database;
    using Services;

    public class ChannelHandler : BaseHandler
    {
        private readonly ChannelService _channel;
        private readonly HashSet<ulong> _channels;
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;
        private readonly LogsService _logs;

        public ChannelHandler(DiscordSocketClient client, ChannelService channel, IDbLanguage language, LocalizationService localization, LogsService logs) : base(client)
        {
            _channel = channel;
            _language = language;
            _localization = localization;
            _logs = logs;
            _channels = new HashSet<ulong>();
        }

        public override Task Initialize()
        {
            Client.UserVoiceStateUpdated += HandleChannelAsync;
            return Task.CompletedTask;
        }

        private async Task LoadLanguage(ulong serverId)
        {
            await _localization.Load(await _language.GetLanguage(serverId));
        }

        private async Task HandleChannelAsync(SocketUser user, SocketVoiceState state1, SocketVoiceState state2)
        {
#if DEBUG
            if (!user.Id.Equals(255453041531158538)) return;
#endif
            try
            {
                var guildUser = user as SocketGuildUser;
                await LeaveChannel(state1.VoiceChannel, guildUser).ConfigureAwait(false);
                await JoinChannel(state2.VoiceChannel, guildUser).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                await _logs.Write("Crashes", $"Failed to handle UserVoiceStateUpdated. {e.Message}, {e.StackTrace}");
            }
        }

        private async Task LeaveChannel(SocketVoiceChannel channel, SocketGuildUser user)
        {
            if (channel == null) return;
            try
            {
                if (channel.Users.Count > 0) return;

                while (_channels.Contains(channel.Id)) await Task.Delay(100);

                if (!await _channel.IsGeneratedChannel(channel)) return;
                await _logs.Write("Channels", channel.Guild, $"{user.Nickname()} left channel '{channel.Name}'.");
                if (channel.Guild.GetChannel(channel.Id) == null) return;
                await channel.DeleteAsync();
                await _channel.RemoveGeneratedChannel(channel.Guild.Id, channel.Id);
            }
            catch (Exception e)
            {
                await _logs.Write("Crashes",
                    $"LeaveChannel crashed. ({channel.Guild.Id}) {channel.Id}, {channel.Name}. Stacktrace: {e}");
            }
        }

        private async Task JoinChannel(SocketVoiceChannel channel, SocketGuildUser user)
        {
            if (channel == null || user == null) return;
            RestVoiceChannel newChannel = null;
            try
            {
                var auto = await _channel.LoadAuto(channel.Guild.Id);
                if (channel.Name.StartsWith(auto.Prefix, StringComparison.OrdinalIgnoreCase))
                {
                    newChannel = await DuplicateChannel(channel, user, auto.Name);
                    await _channel.AddGeneratedChannel(channel.Guild.Id, newChannel.Id);
                    _channels.Remove(newChannel.Id);
                    return;
                }

                var perma = await _channel.LoadPerma(channel.Guild.Id);
                if (channel.Name.StartsWith(perma.Prefix, StringComparison.OrdinalIgnoreCase))
                {
                    newChannel = await DuplicateChannel(channel, user, perma.Name);
                    _channels.Remove(newChannel.Id);
                }
            }
            catch (HttpException httpException)
            {
                if (!httpException.Message.Contains("error 50013: Missing Permissions"))
                {
                    await _logs.Write("Crashes",
                        $"JoinChannel crashed. ({channel.Guild.Id}) {channel.Id}, {channel.Name}. Stacktrace: {httpException}");
                    if (newChannel != null)
                    {
                        await newChannel.DeleteAsync();
                        await _channel.RemoveGeneratedChannel(newChannel.GuildId, newChannel.Id);
                        _channels.Remove(newChannel.Id);
                    }

                    return;
                }

                var pmChannel = await channel.Guild.Owner.GetOrCreateDMChannelAsync();
                await LoadLanguage(channel.Guild.Id);
                await pmChannel.SendMessageAsync(_localization.GetMessage("Channel no permission", channel.Guild.Name,
                    channel.Name));
                if (newChannel != null)
                {
                    await newChannel.DeleteAsync();
                    await _channel.RemoveGeneratedChannel(newChannel.GuildId, newChannel.Id);
                    _channels.Remove(newChannel.Id);
                }
            }
            catch (Exception e)
            {
                await _logs.Write("Crashes",
                    $"JoinChannel crashed. ({channel.Guild.Id}) {channel.Id}, {channel.Name}. Stacktrace: {e}");
                if (newChannel != null)
                {
                    await newChannel.DeleteAsync();
                    await _channel.RemoveGeneratedChannel(newChannel.GuildId, newChannel.Id);
                    _channels.Remove(newChannel.Id);
                }
            }
        }

        private async Task<RestVoiceChannel> DuplicateChannel(SocketVoiceChannel channel, SocketGuildUser user, string name)
        {
            await _logs.Write("Channels", channel.Guild, $"{user.Username} joined channel '{channel.Name}'.");
            var newChannel = await channel.Guild.CreateVoiceChannelAsync(name, p =>
            {
                p.Bitrate = channel.Bitrate;
                p.CategoryId = channel.CategoryId;
                p.UserLimit = channel.UserLimit;
            });
            _channels.Add(newChannel.Id);

            await user.ModifyAsync(u => u.Channel = newChannel);
            foreach (var p in channel.PermissionOverwrites)
                await newChannel.AddPermissionOverwriteAsync(channel.Guild.GetRole(p.TargetId), p.Permissions);
            await newChannel.AddPermissionOverwriteAsync(user,
                new OverwritePermissions(PermValue.Inherit, PermValue.Allow));
            return newChannel;
        }
    }
}