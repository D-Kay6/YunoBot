using System;
using System.Collections.Generic;
using Discord;
using Discord.WebSocket;
using System.Linq;
using System.Threading.Tasks;
using Logic.Exceptions;
using Logic.Extentions;
using Victoria;
using Victoria.Entities;

namespace Logic.Services
{
    public class AudioService
    {
        private readonly DiscordSocketClient _client;
        private readonly LavaRestClient _lavaRestClient;
        private readonly LavaSocketClient _lavaClient;
        private LavaPlayer _player;

        private Dictionary<ulong, Queue<IPlayable>> _queues;
        private Queue<IPlayable> _queue;

        public bool IsPlaying => _player?.IsPlaying ?? false;
        public IVoiceChannel VoiceChannel => _player?.VoiceChannel;
        public IMessageChannel TextChannel { get; private set; }
        public LavaTrack CurrentTrack => _player?.CurrentTrack;


        public AudioService(DiscordSocketClient client)
        {
            _client = client;
            _lavaRestClient = new LavaRestClient();
            _lavaClient = new LavaSocketClient();
            _queues = new Dictionary<ulong, Queue<IPlayable>>();
            _client.Ready += OnReady;
        }


        private async Task OnReady()
        {
            await _lavaClient.StartAsync(_client);
            _lavaClient.OnTrackException += OnTrackException;
            _lavaClient.OnTrackStuck += OnTrackStuck;
            _lavaClient.OnTrackFinished += OnTrackFinished;
        }

        private async Task OnTrackException(LavaPlayer player, LavaTrack track, string exception)
        {
            _player = player;
            await PlayNextTrack();
        }

        private async Task OnTrackStuck(LavaPlayer player, LavaTrack track, long whatefs)
        {
            _player = player;
            await PlayNextTrack();
        }

        private async Task OnTrackFinished(LavaPlayer player, LavaTrack track, TrackEndReason reason)
        {
            _player = player;
            switch (reason)
            {
                case TrackEndReason.Finished:
                    await PlayNextTrack();
                    break;
                case TrackEndReason.Replaced:
                    break;
                case TrackEndReason.LoadFailed:
                    await PlayNextTrack();
                    break;
                case TrackEndReason.Cleanup:
                    break;
                case TrackEndReason.Stopped:
                    break;
            }
        }
        

        private async Task PlayNextTrack()
        {
            _queue = _queues[_player.VoiceChannel.GuildId];
            if (_queue.Count < 1)
            {
                await EndPlayer();
                return;
            }

            var song = _queue.Dequeue();
            TextChannel = song.TextChannel;
            await song.TextChannel.SendMessageAsync($"Now playing `{song.Track.Title}` requested by {song.Requester.Nickname ?? song.Requester.Username}");
            await _player.PlayAsync(song.Track);
        }

        private async Task EndPlayer()
        {
            if (_player == null) throw new InvalidPlayerException();
            _queues.Remove(_player.VoiceChannel.GuildId);
            await _lavaClient.DisconnectAsync(_player.VoiceChannel);
        }


        public void BeforeExecute(ulong guildId)
        {
            _player = _lavaClient.GetPlayer(guildId);
            if (_queues.TryGetValue(guildId, out _queue)) return;
            _queue = new Queue<IPlayable>();
            _queues.Add(guildId, _queue);
        }


        public async Task<SearchResult> GetTracks(string query)
        {
            return await _lavaRestClient.SearchYouTubeAsync(query);
        }

        public async Task<LavaTrack> GetTrack(string query)
        {
            var result = await GetTracks(query);
            if (result.LoadType != LoadType.SearchResult) return null;
            return result.Tracks.FirstOrDefault(t => query.Contains(t.Id)) ?? result.Tracks.First();
        }


        public async Task<int> Queue(IPlayable song)
        {
            _queue.Enqueue(song);

            if (IsPlaying) return _queue.Count;

            _player = await _lavaClient.ConnectAsync(song.Requester.VoiceChannel);
            await PlayNextTrack();
            await _player.SetVolumeAsync(50);
            return 0;
        }

        public async Task<bool> Play()
        {
            if (_player == null) throw new InvalidPlayerException();
            if (!_player.IsPaused) return false;
            await _player.PauseAsync();
            return true;
        }

        public async Task<bool> Pause()
        {
            if (_player == null) throw new InvalidPlayerException();
            if (_player.IsPaused) return false;
            await _player.PauseAsync();
            return true;
        }

        public void Shuffle()
        {
            _queue.Shuffle();
        }

        public async Task Skip()
        {
            if (_player == null) throw new InvalidPlayerException();
            await _player.StopAsync();
            await PlayNextTrack();
        }

        public async Task Stop()
        {
            if (_player == null) throw new InvalidPlayerException();
            await _player.StopAsync();
            _queue.Clear();
            await EndPlayer();
        }

        public void Clear()
        {
            if (_player == null) throw new InvalidPlayerException();
            _queue.Clear();
        }

        public IReadOnlyCollection<IPlayable> GetQueue()
        {
            return new List<IPlayable>(_queue);
        }
    }
}