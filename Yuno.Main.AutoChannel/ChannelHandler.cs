using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Yuno.Data.Core.Interfaces;
using Yuno.Logic;

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
            if (state1.VoiceChannel != null) LeaveChannel(state1.VoiceChannel);
            
            if (state2.VoiceChannel != null) JoinChannel(state2.VoiceChannel, (SocketGuildUser)user);
        }

        private async void LeaveChannel(SocketVoiceChannel channel)
        {
            var autoChannel = Logic.AutoChannel.Load(channel.Guild.Id);
            if (!autoChannel.IsControlledChannel(channel.Id)) return;
            if (channel.Users.Count != 0) return;
            autoChannel.RemoveChannel(channel.Id);
            autoChannel.Save();
            channel.DeleteAsync();
        }

        private async void JoinChannel(SocketVoiceChannel channel, SocketGuildUser user)
        {
            var autoChannel = Logic.AutoChannel.Load(channel.Guild.Id);
            if (!autoChannel.IsAutoChannel(channel.Name)) return;
            var newChannel = await channel.Guild.CreateVoiceChannelAsync("--channel");
            await newChannel.ModifyAsync(v =>
            {
                v.Bitrate = channel.Bitrate;
                v.CategoryId = channel.CategoryId;
                v.UserLimit = channel.UserLimit;
            });
            await (user.ModifyAsync(a => a.Channel = newChannel));
            autoChannel.AddChannel(newChannel.Id);
            autoChannel.Save();
            var tasks = channel.PermissionOverwrites.Select(o => newChannel.AddPermissionOverwriteAsync(channel.Guild.GetRole(o.TargetId), o.Permissions));
            Task.WhenAll(tasks);
            
            newChannel.AddPermissionOverwriteAsync(user, new OverwritePermissions(PermValue.Inherit, PermValue.Allow));
            //foreach (var overwrite in channel.PermissionOverwrites)
            //{
            //    var role = channel.Guild.GetRole(overwrite.TargetId);
            //    await newChannel.AddPermissionOverwriteAsync(role, overwrite.Permissions);
            //}
        }
    }
}
