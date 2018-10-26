using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Yuno.Data.Core.Interfaces;
using Yuno.Data.Core.Structs;

namespace Yuno.Main.AutoChannel
{
    public class ChannelHandler
    {
        private ISerializer _serializer;
        private DiscordSocketClient _client;
        private Dictionary<ulong, AutoChannel> _autoChannels;

        public ChannelHandler(ISerializer serializer)
        {
            this._serializer = serializer;
            this._autoChannels = new Dictionary<ulong, AutoChannel>();
        }

        public async Task Initialize(DiscordSocketClient client)
        {
            this._client = client;
            _client.UserVoiceStateUpdated += HandleChannelAsync;
        }

        private async Task HandleChannelAsync(SocketUser user, SocketVoiceState state1, SocketVoiceState state2)
        {
            SocketVoiceChannel oldChannel;
            RestVoiceChannel newChannel;

            #region Leave channel

            if (state1.VoiceChannel != null)
            {
                oldChannel = state1.VoiceChannel;
                var autoChannel = GetAutoChannel(oldChannel.Guild.Id);
                if (autoChannel.IsGeneratedChannel(oldChannel.Id))
                {
                    if (oldChannel.Users.Count != 0) return;
                    autoChannel.RemoveChannel(oldChannel.Id);
                    _serializer.Write(oldChannel.Guild.Id, new Persistence(autoChannel.Channels, autoChannel.AutoChannelIcon));
                    await oldChannel.DeleteAsync();
                    return;
                }
            } 
            
            #endregion

            #region Join channel

            if (state2.VoiceChannel != null)
            {
                var autoChannel = GetAutoChannel(state2.VoiceChannel.Guild.Id);
                if (autoChannel.IsAutoChannel(state2.VoiceChannel.Name))
                {
                    oldChannel = state2.VoiceChannel;
                    newChannel = await oldChannel.Guild.CreateVoiceChannelAsync("--channel");
                    await newChannel.ModifyAsync(v => 
                    {
                        v.Bitrate = oldChannel.Bitrate;
                        v.CategoryId = oldChannel.CategoryId;
                        v.UserLimit = oldChannel.UserLimit;
                    });
                    foreach (var overwrite in oldChannel.PermissionOverwrites)
                    {
                        var role = oldChannel.Guild.GetRole(overwrite.TargetId);
                        await newChannel.AddPermissionOverwriteAsync(role, overwrite.Permissions);
                    }

                    await newChannel.AddPermissionOverwriteAsync(user, new OverwritePermissions(PermValue.Inherit, PermValue.Allow));

                    autoChannel.AddChannel(newChannel.Id);
                    _serializer.Write(state2.VoiceChannel.Guild.Id, new Persistence(autoChannel.Channels, autoChannel.AutoChannelIcon));

                    await ((SocketGuildUser)user).ModifyAsync(a => a.Channel = newChannel);
                }
            }
            
            #endregion
        }

        private AutoChannel GetAutoChannel(ulong id)
        {
            if (_autoChannels.ContainsKey(id)) return _autoChannels[id];
            var autoChannel = new AutoChannel();
            autoChannel.Load(_serializer.Read(id));
            _autoChannels.Add(id, autoChannel);
            return autoChannel;
        }
    }
}
