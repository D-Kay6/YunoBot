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
using Victoria.Entities.Enums;

namespace Logic.Services
{
    public class AudioService
    {
        private readonly DiscordSocketClient _client;
        private readonly Lavalink _lavalink;

        private LavaNode _node;
        private LavaPlayer _player;

        private Dictionary<ulong, Queue<IPlayable>> _queues;
        private Queue<IPlayable> _queue;

        public bool IsPlaying => _player?.IsPlaying ?? false;
        public IVoiceChannel VoiceChannel => _player?.VoiceChannel;
        public IMessageChannel TextChannel => _player?.TextChannel;
        public LavaTrack CurrentTrack => _player?.CurrentTrack;


        public AudioService(DiscordSocketClient client)
        {
            _client = client;
            _lavalink = new Lavalink();
            _queues = new Dictionary<ulong, Queue<IPlayable>>();
            _client.Ready += OnReady;
        }


        private async Task OnReady()
        {
            _node = await _lavalink.AddNodeAsync(_client);
            _node.TrackException += OnTrackException;
            _node.TrackStuck += OnTrackStuck;
            _node.TrackFinished += OnTrackFinished;
        }

        private async Task OnTrackStuck(LavaPlayer player, LavaTrack track, long whatefs)
        {
            _player = player;
            await PlayNextTrack();
        }

        private async Task OnTrackException(LavaPlayer player, LavaTrack track, string exception)
        {
            _player = player;
            await PlayNextTrack();
        }

        private async Task OnTrackFinished(LavaPlayer player, LavaTrack track, TrackReason reason)
        {
            _player = player;
            switch (reason)
            {
                case TrackReason.Finished:
                    await PlayNextTrack();
                    break;
                case TrackReason.Replaced:
                    break;
                case TrackReason.LoadFailed:
                    await PlayNextTrack();
                    break;
                case TrackReason.Cleanup:
                    break;
                case TrackReason.Stopped:
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
            _player.TextChannel = song.TextChannel;
            await song.TextChannel.SendMessageAsync($"Now playing `{song.Track.Title}` requested by {song.Requester.Nickname ?? song.Requester.Username}");
            await _player.PlayAsync(song.Track);
        }

        private async Task EndPlayer()
        {
            if (_player == null) throw new InvalidPlayerException();
            _queues.Remove(_player.VoiceChannel.GuildId);
            await _node.DisconnectAsync(_player.VoiceChannel.GuildId);
        }


        public void BeforeExecute(ulong guildId)
        {
            _player = _node.GetPlayer(guildId);
            if (_queues.TryGetValue(guildId, out _queue)) return;
            _queue = new Queue<IPlayable>();
            _queues.Add(guildId, _queue);
        }


        public async Task<LavaResult> GetTracks(string query)
        {
            return await _node.SearchYouTubeAsync(query);
        }

        public async Task<LavaTrack> GetTrack(string query)
        {
            var result = await GetTracks(query);
            if (result.LoadResultType != LoadResultType.SearchResult) return null;
            return result.Tracks.FirstOrDefault(t => query.Contains(t.Id)) ?? result.Tracks.First();
        }


        public async Task<int> Queue(IPlayable song)
        {
            _queue.Enqueue(song);

            if (IsPlaying) return _queue.Count;

            _player = await _node.ConnectAsync(song.Requester.VoiceChannel);
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