using Discord.Rest;
using Discord.WebSocket;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Channels
{
    public class ChannelHandler
    {
        private DiscordSocketClient _client;
        private HashSet<ulong> _channels;

        public async Task Initialize(DiscordSocketClient client)
        {
            this._client = client;
            this._channels = new HashSet<ulong>();
            _client.UserVoiceStateUpdated += HandleChannelAsync;
        }

        private async Task HandleChannelAsync(SocketUser user, SocketVoiceState state1, SocketVoiceState state2)
        {
            SocketVoiceChannel oldChannel;
            RestVoiceChannel newChannel;

            #region Leave channel

            if (state1.VoiceChannel != null && _channels.Contains(state1.VoiceChannel.Id))
            {
                oldChannel = state1.VoiceChannel;
                if (oldChannel.Users.Count != 0) return;
                _channels.Remove(oldChannel.Id);
                await oldChannel.DeleteAsync();
                return;
            }
            
            #endregion

            #region Join channel

            if (state2.VoiceChannel != null && char.ConvertToUtf32(state2.VoiceChannel.Name, 0) == 10133)
            {
                oldChannel = state2.VoiceChannel;
                newChannel = await oldChannel.Guild.CreateVoiceChannelAsync("--channel");
                await newChannel.ModifyAsync(v => 
                {
                    v.Bitrate = oldChannel.Bitrate;
                    v.CategoryId = oldChannel.CategoryId;
                    v.UserLimit = oldChannel.UserLimit;
                });
                _channels.Add(newChannel.Id);

                var guildUser = (SocketGuildUser)user;
                await guildUser.ModifyAsync(a => a.Channel = newChannel);
            }
            
            #endregion
        }
    }
}
