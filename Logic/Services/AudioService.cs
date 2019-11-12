using Discord;
using Discord.WebSocket;
using Logic.Exceptions;
using Logic.Extensions;
using Logic.Services.Music;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Victoria;
using Victoria.Enums;
using Victoria.EventArgs;
using Victoria.Responses.Rest;

namespace Logic.Services
{
    public class AudioService
    {
        private readonly DiscordSocketClient _client;
        private readonly LavaConfig _lavaConfig;
        private readonly LavaNode _lavaNode;
        private readonly Dictionary<ulong, Queue<IPlayable>> _queues;

        private LavaPlayer _player;
        private Queue<IPlayable> _queue;
        private Localization.Localization _lang;

        public bool IsPlaying => _player != null && _player.PlayerState == PlayerState.Playing || _player.PlayerState == PlayerState.Paused;
        public bool IsPaused => _player != null && _player.PlayerState == PlayerState.Paused;
        public PlayerState State => _player?.PlayerState ?? PlayerState.Disconnected;
        public IVoiceChannel VoiceChannel => _player?.VoiceChannel;
        public ITextChannel TextChannel => _player?.TextChannel;
        public LavaTrack CurrentTrack => _player?.Track;
        
        public AudioService(DiscordSocketClient client)
        {
            _client = client;
            _lavaConfig = new LavaConfig();
            _lavaNode = new LavaNode(_client, _lavaConfig);
            _queues = new Dictionary<ulong, Queue<IPlayable>>();
            _client.Ready += OnReady;
        }
        

        private async Task OnReady()
        {
            await _lavaNode.ConnectAsync();
            _lavaNode.OnTrackException += OnTrackException;
            _lavaNode.OnTrackStuck += OnTrackStuck;
            _lavaNode.OnTrackEnded += OnTrackEnded;
        }


        private async Task OnTrackException(TrackExceptionEventArgs e)
        {
            //_player = e.Player;
            //_lang = new Localization.Localization(_player.VoiceChannel.GuildId);
            await PlayNextTrack();
        }

        private async Task OnTrackStuck(TrackStuckEventArgs e)
        {
            //_player = e.Player;
            //_lang = new Localization.Localization(_player.VoiceChannel.GuildId);
            await PlayNextTrack();
        }

        private async Task OnTrackEnded(TrackEndedEventArgs e)
        {
            _player = e.Player;
            _lang = new Localization.Localization(_player.VoiceChannel.GuildId);
            switch (e.Reason)
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
                    await EndPlayer();
                    break;
            }
        }
        

        private async Task PlayNextTrack()
        {
            if (_queue.Count == 0)
            {
                await EndPlayer();
                return;
            }

            var song = _queue.Dequeue();
            _lavaNode.UpdateTextChannel(song.Guild, song.TextChannel);
            await song.TextChannel.SendMessageAsync(_lang.GetMessage("Music now playing", song.Track.Title, song.Requester.Nickname()));
            await _player.PlayAsync(song.Track);
        }

        private async Task EndPlayer()
        {
            if (_player == null) throw new InvalidPlayerException();
            _queue.Clear();
            await _lavaNode.LeaveAsync(_player.VoiceChannel);

        }


        public void BeforeExecute(IGuild guild)
        {
            _player = _lavaNode.HasPlayer(guild) ? _lavaNode.GetPlayer(guild) : null;
            if (_queues.ContainsKey(guild.Id)) _queue = _queues[guild.Id];
            else
            {
                _queue = new Queue<IPlayable>();
                _queues.Add(guild.Id, _queue);
            }
            _lang = new Localization.Localization(guild.Id);
        }


        public async Task<SearchResponse> GetTracks(string query)
        {
            var result = await _lavaNode.SearchAsync(query);
            if (result.LoadType == LoadType.NoMatches) result = await _lavaNode.SearchYouTubeAsync(query);
            return result;
        }
        
        public async Task Queue(IPlayable song)
        {
            if (_player == null) _player = await _lavaNode.JoinAsync(song.Requester.VoiceChannel);
            _queue.Enqueue(song);
            if (_player.PlayerState == PlayerState.Connected)
            {
                await PlayNextTrack();
                await _player.UpdateVolumeAsync(25);
                return;
            }
            await song.TextChannel.SendMessageAsync(_lang.GetMessage("Music queued song", _queue.Count, song.Track.Title, song.Track.Duration));
        }
        
        public async Task Queue(IEnumerable<IPlayable> songs, IVoiceChannel channel)
        {
            if (IsPlaying)
            {
                songs.Foreach(s => _queue.Enqueue(s));
                return;
            }
            
            _player = await _lavaNode.JoinAsync(channel);
            songs.Foreach(s => _queue.Enqueue(s));
            await PlayNextTrack();
            await _player.UpdateVolumeAsync(25);
        }

        public async Task<bool> Play()
        {
            if (_player == null) throw new InvalidPlayerException();
            if (_player.PlayerState != PlayerState.Paused) return false;
            await _player.ResumeAsync();
            return true;
        }

        public async Task<bool> Pause()
        {
            if (_player == null) throw new InvalidPlayerException();
            if (_player.PlayerState == PlayerState.Paused) return false;
            await _player.PauseAsync();
            return true;
        }

        public void Shuffle()
        {
            _queue.Shuffle();
        }

        public async Task Skip(int amount = 1)
        {
            if (_player == null) throw new InvalidPlayerException();

            for (var i = 0; i < amount - 1; i++) _queue.Dequeue();

            await PlayNextTrack();
        }

        public async Task Stop()
        {
            if (_player == null) throw new InvalidPlayerException();
            await _player.StopAsync();
        }

        public void Clear()
        {
            if (_player == null) throw new InvalidPlayerException();
            _queue.Clear();
        }

        public IReadOnlyCollection<IPlayable> GetQueue()
        {
            return _queue.ToList();
        }

        public async Task ChangeVolume(ushort volume)
        {
            if (_player == null) throw new InvalidPlayerException();
            if (_player.PlayerState != PlayerState.Playing) throw new InvalidPlayerException();
            await _player.UpdateVolumeAsync(volume);
        }
    }
}