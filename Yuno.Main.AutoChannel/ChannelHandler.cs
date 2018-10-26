using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Yuno.Data.Core.Interfaces;
using Yuno.Logic;
using Yuno.Logic.Core;

namespace Yuno.Main.AutoChannel
{
    public class ChannelHandler
    {
        private ISerializer _serializer;
        private DiscordSocketClient _client;

        public ChannelHandler(ISerializer serializer)
        {
            this._serializer = serializer;
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
                var persistence = DomainTranslator.Translate(_serializer.Read(oldChannel.Guild.Id));
                var autoChannel = (AutoChannelLogic)persistence.AutoChannel;
                if (autoChannel.IsControlledChannel(oldChannel.Id))
                {
                    if (oldChannel.Users.Count != 0) return;
                    autoChannel.RemoveChannel(oldChannel.Id);
                    _serializer.Write(oldChannel.Guild.Id, persistence);
                    await oldChannel.DeleteAsync();
                    return;
                }
            } 
            
            #endregion

            #region Join channel

            if (state2.VoiceChannel != null)
            {
                oldChannel = state2.VoiceChannel;
                var persistence = DomainTranslator.Translate(_serializer.Read(oldChannel.Guild.Id));
                var autoChannel = (AutoChannelLogic)persistence.AutoChannel;
                if (autoChannel.IsAutoChannel(oldChannel.Name))
                {
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
                    _serializer.Write(state2.VoiceChannel.Guild.Id, persistence);

                    await ((SocketGuildUser)user).ModifyAsync(a => a.Channel = newChannel);
                }
            }
            
            #endregion
        }
    }
}
