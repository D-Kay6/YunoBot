using Core.Enum;
using Discord;
using Discord.Net;
using Discord.Rest;
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
        private readonly DynamicChannelService _channel;
        private readonly HashSet<ulong> _channels;
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;

        public ChannelHandler(DiscordShardedClient client, LogsService logs, DynamicChannelService channel, IDbLanguage language, LocalizationService localization) : base(client, logs)
        {
            _channel = channel;
            _language = language;
            _localization = localization;
            _channels = new HashSet<ulong>();
        }

        public override Task Initialize()
        {
            base.Initialize();
            Client.ShardReady += Ready;
            Client.UserVoiceStateUpdated += HandleChannelAsync;
            return Task.CompletedTask;
        }

        protected override async Task Ready(DiscordSocketClient client)
        {
            await base.Ready(client);
            //foreach (var guild in client.Guilds)
            //{
            //    var generatedChannels = await _channel.ListGeneratedChannels(guild.Id);
            //    foreach (var generatedChannel in generatedChannels)
            //    {
            //        try
            //        {
            //            if (guild.GetChannel(generatedChannel.ChannelId) != null) continue;
            //            await _channel.RemoveGeneratedChannel(generatedChannel);
            //        }
            //        catch
            //        {
            //            //ignore and continue
            //        }
            //    }
            //}
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
            if (state1.VoiceChannel == state2.VoiceChannel) return;

            try
            {
                var guildUser = user as SocketGuildUser;
                await LeaveChannel(state1.VoiceChannel, guildUser).ConfigureAwait(false);
                await JoinChannel(state2.VoiceChannel, guildUser).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                await Logs.Write("Crashes", "Failed to handle UserVoiceStateUpdated.", e);
            }
        }

        private async Task LeaveChannel(SocketVoiceChannel channel, SocketGuildUser user)
        {
            if (channel == null) return;
            try
            {
                if (channel.Users.Count > 0) return;
                if (!await _channel.IsGeneratedChannel(channel)) return;
                while (_channels.Contains(channel.Id)) await Task.Delay(100);

                await Logs.Write("Channels", $"{user.Nickname()} left channel '{channel.Name}'.", channel.Guild);
                if (channel.Guild.GetChannel(channel.Id) == null) return;
                await channel.DeleteAsync();
                await _channel.RemoveGeneratedChannel(channel.Guild.Id, channel.Id);
            }
            catch (Exception e)
            {
                await Logs.Write("Crashes",
                    $"LeaveChannel crashed. ({channel.Guild.Id}) {channel.Id}, {channel.Name}.", e);
            }
        }

        private async Task JoinChannel(SocketVoiceChannel channel, SocketGuildUser user)
        {
            if (channel == null) return;

            var failed = false;
            RestVoiceChannel newChannel = null;
            try
            {
                var auto = await _channel.Load(channel.Guild.Id, AutomationType.Temporary);
                if (channel.Name.StartsWith(auto.Prefix, StringComparison.OrdinalIgnoreCase))
                {
                    newChannel = await DuplicateChannel(channel, user, auto.Name);
                    await _channel.AddGeneratedChannel(channel.Guild.Id, newChannel.Id);
                    _channels.Remove(newChannel.Id);
                    return;
                }

                var perma = await _channel.Load(channel.Guild.Id, AutomationType.Permanent);
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
                    await Logs.Write("Crashes",
                        $"JoinChannel crashed. ({channel.Guild.Id}) {channel.Id}, {channel.Name}.", httpException);
                }
                else
                {
                    var pmChannel = await channel.Guild.Owner.GetOrCreateDMChannelAsync();
                    await LoadLanguage(channel.Guild.Id);
                    await pmChannel.SendMessageAsync(_localization.GetMessage("Channel no permission",
                        channel.Guild.Name, channel.Name));
                }

                failed = true;
            }
            catch (Exception e)
            {
                await Logs.Write("Crashes",
                    $"JoinChannel crashed. ({channel.Guild.Id}) {channel.Id}, {channel.Name}.", e);
                failed = true;
            }

            if (failed)
                if (newChannel != null)
                {
                    await newChannel.DeleteAsync();
                    try
                    {
                        await _channel.RemoveGeneratedChannel(newChannel.GuildId, newChannel.Id);
                    }
                    catch (Exception)
                    {
                        //ignore
                    }

                    _channels.Remove(newChannel.Id);
                }
        }

        private async Task<RestVoiceChannel> DuplicateChannel(SocketVoiceChannel channel, SocketGuildUser user, string name)
        {
            await Logs.Write("Channels", $"{user.Username} joined channel '{channel.Name}'.", channel.Guild);
            var newChannel = await channel.Guild.CreateVoiceChannelAsync(name, p =>
            {
                p.Bitrate = channel.Bitrate;
                p.CategoryId = channel.CategoryId;
                p.UserLimit = channel.UserLimit;
            });
            _channels.Add(newChannel.Id);

            try
            {
                if (user.VoiceChannel != null)
                    await user.ModifyAsync(u => u.Channel = newChannel);
            }
            catch (Exception)
            {
                //ignore
            }

            foreach (var p in channel.PermissionOverwrites)
                await newChannel.AddPermissionOverwriteAsync(channel.Guild.GetRole(p.TargetId), p.Permissions);

            await newChannel.AddPermissionOverwriteAsync(user,
                new OverwritePermissions(PermValue.Inherit, PermValue.Allow));
            return newChannel;
        }
    }
}