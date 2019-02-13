using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Victoria;
using Victoria.Entities;
using Victoria.Entities.Enums;

namespace Yuno.Main.Music
{
    public class AudioService
    {
        private readonly DiscordSocketClient _client;
        private readonly Lavalink _lavalink;

        private LavaNode _node;
        private LavaPlayer _player;

        public bool IsPlaying => _player?.IsPlaying ?? false;
        public IVoiceChannel VoiceChannel => _player?.VoiceChannel;
        public LavaTrack CurrentTrack => _player?.CurrentTrack;

        public AudioService(DiscordSocketClient client)
        {
            _client = client;
            _lavalink = new Lavalink();
            _client.Ready += OnReady;
        }

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
            if (_player != null)
            {
                _player.Queue.Enqueue(song.Track);
                return;
            }

            _player = await _node.ConnectAsync(song.Requester.VoiceChannel);
            await _player.SetVolumeAsync(25);
            await _player.PlayAsync(song.Track);
        }

        public async Task<TrackReason> Skip()
        {
            if (_player == null) return;
            TrackReason.
            var nextTrack = _player.Queue.
            await _player.SkipAsync();
        }

        public async Task Stop()
        {
            if (_player == null) return;
            await _player.StopAsync();
            _player.Queue.Clear();
        }

        public void Clear()
        {
            _player?.Queue.Clear();
        }
    }
}
