using System.Collections.Generic;
using Discord;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;
using Victoria;
using Victoria.Entities;
using Victoria.Entities.Enums;

namespace Logic.Services
{
    public class AudioService
    {
        private readonly DiscordSocketClient _client;
        private readonly Lavalink _lavalink;

        private LavaNode _node;
        private LavaPlayer _player;

        private Queue<IPlayable> _queue;

        public AudioService(DiscordSocketClient client)
        {
            _client = client;
            _lavalink = new Lavalink();
            _queue = new Queue<IPlayable>();
            _client.Ready += OnReady;
        }

        public bool IsPlaying => _player?.IsPlaying ?? false;
        public IVoiceChannel VoiceChannel => _player?.VoiceChannel;
        public LavaTrack CurrentTrack => _player?.CurrentTrack;

        private async Task OnReady()
        {
            _node = await _lavalink.AddNodeAsync(_client);
            _node.TrackException += OnTrackException;
            _node.TrackStuck += OnTrackStuck;
            _node.TrackFinished += OnTrackFinished;
            _node.SocketClosed += OnSocketClosed;
        }

        private async Task OnSocketClosed(int arg1, string arg2, bool arg3)
        {
        }

        private async Task OnTrackStuck(LavaPlayer player, LavaTrack track, long whatefs)
        {
        }

        private async Task OnTrackException(LavaPlayer player, LavaTrack track, string exception)
        {
        }

        private async Task OnTrackFinished(LavaPlayer player, LavaTrack track, TrackReason reason)
        {
            switch (reason)
            {
                case TrackReason.Finished:
                case TrackReason.Replaced:
                case TrackReason.LoadFailed:
                case TrackReason.Cleanup:
                    break;
                case TrackReason.Stopped:
                    break;
            }
        }

        private async Task<bool> PlayNextTrack()
        {
            if (_player == null) return false;
            if (_player.Queue.Count < 1)
            {
                await EndPlayer();
                return false;
            }
            await _player.PlayAsync(_player.Queue.Dequeue());
            return true;
        }

        private async Task EndPlayer()
        {
            if (_player == null) return;
            await _node.DisconnectAsync(_player.VoiceChannel.GuildId);
        }

        public void BeforeExecute(ulong guildId)
        {
            _player = _node.GetPlayer(guildId);
        }

        public async Task<LavaTrack> GetTrack(string query)
        {
            var result = await _node.SearchYouTubeAsync(query);
            return result.LoadResultType == LoadResultType.SearchResult ? result.Tracks.First() : null;
        }

        public async Task<LavaResult> GetTracks(string query)
        {
            return await _node.SearchYouTubeAsync(query);
        }

        public async Task Queue(IPlayable song)
        {
            if (IsPlaying)
            {
                _player.Queue.Enqueue(song.Track);
                return;
            }

            _player = await _node.ConnectAsync(song.Requester.VoiceChannel);
            await _player.PlayAsync(song.Track);
            await _player.SetVolumeAsync(25);
        }

        public async Task<bool> Skip()
        {
            if (_player == null) return false;
            await _player.StopAsync();
            if (_player.Queue.Count < 1) return false;
            await _player.PlayAsync(_player.Queue.Dequeue());
            return true;
        }

        public async Task Stop()
        {
            if (_player == null) return;
            await _player.StopAsync();
            _player.Queue.Clear();
            await EndPlayer();
        }

        public void Clear()
        {
            _player?.Queue.Clear();
        }
    }
}