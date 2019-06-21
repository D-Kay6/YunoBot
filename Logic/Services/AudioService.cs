using Discord;
using Discord.WebSocket;
using Logic.Exceptions;
using Logic.Extentions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Victoria;
using Victoria.Entities;
using SearchResult = Victoria.Entities.SearchResult;

namespace Logic.Services
{
    public class AudioService
    {
        private readonly DiscordSocketClient _client;
        private readonly LavaRestClient _lavaRestClient;
        private readonly LavaSocketClient _lavaClient;
        private LavaPlayer _player;
        private Localization.Localization _lang;

        public bool IsPlaying => _player?.IsPlaying ?? false;
        public IVoiceChannel VoiceChannel => _player?.VoiceChannel;
        public ITextChannel TextChannel => _player?.TextChannel;
        public LavaTrack CurrentTrack => _player?.CurrentTrack;
        
        public AudioService(DiscordSocketClient client)
        {
            _client = client;
            _lavaRestClient = new LavaRestClient();
            _lavaClient = new LavaSocketClient();
            _client.Ready += OnReady;
        }
        

        private async Task OnReady()
        {
            await _lavaClient.StartAsync(_client, new Configuration
            {
                AutoDisconnect = true
            });
            _lavaClient.OnTrackException += OnTrackException;
            _lavaClient.OnTrackStuck += OnTrackStuck;
            _lavaClient.OnTrackFinished += OnTrackFinished;
        }


        private async Task OnTrackStuck(LavaPlayer player, LavaTrack track, long whatefs)
        {
            _player = player;
            _lang = new Localization.Localization(_player.VoiceChannel.GuildId);
            await PlayNextTrack();
        }

        private async Task OnTrackException(LavaPlayer player, LavaTrack track, string exception)
        {
            _player = player;
            _lang = new Localization.Localization(_player.VoiceChannel.GuildId);
            await PlayNextTrack();
        }

        private async Task OnTrackFinished(LavaPlayer player, LavaTrack track, TrackEndReason reason)
        {
            _player = player;
            _lang = new Localization.Localization(_player.VoiceChannel.GuildId);
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
                    await EndPlayer();
                    break;
            }
        }
        

        private async Task PlayNextTrack()
        {
            if (_player.Queue.Count == 0)
            {
                await EndPlayer();
                return;
            }
            var song = _player.Queue.Dequeue() as IPlayable;
            _lavaClient.UpdateTextChannel(song.Guild.Id, song.TextChannel);
            await song.TextChannel.SendMessageAsync(_lang.GetMessage("Music now playing", song.Track.Title, song.Requester.Nickname()));
            await _player.PlayAsync(song.Track);
        }

        private async Task EndPlayer()
        {
            if (_player == null) throw new InvalidPlayerException();
            _player.Queue.Clear();
            await _lavaClient.DisconnectAsync(_player.VoiceChannel);
        }


        public void BeforeExecute(ulong serverId)
        {
            _player = _lavaClient.GetPlayer(serverId);
            _lang = new Localization.Localization(serverId);
        }


        public async Task<SearchResult> GetTracks(string query)
        {
            var result = await _lavaRestClient.SearchTracksAsync(query);
            if (result.LoadType == LoadType.NoMatches) result = await _lavaRestClient.SearchYouTubeAsync(query);
            return result;
        }
        
        public async Task Queue(IPlayable song)
        {
            if (_player == null) _player = await _lavaClient.ConnectAsync(song.Requester.VoiceChannel);
            _player.Queue.Enqueue(song);
            if (!_player.IsPlaying)
            {
                await PlayNextTrack();
                await _player.SetVolumeAsync(25);
                return;
            }
            await song.TextChannel.SendMessageAsync(_lang.GetMessage("Music queued song", _player.Queue.Count, song.Track.Title, song.Track.Length));
        }
        
        public async Task Queue(IEnumerable<IPlayable> songs, IVoiceChannel channel)
        {
            if (IsPlaying)
            {
                songs.Foreach(s => _player.Queue.Enqueue(s));
                return;
            }
            
            _player = await _lavaClient.ConnectAsync(channel);
            songs.Foreach(s => _player.Queue.Enqueue(s));
            await PlayNextTrack();
            await _player.SetVolumeAsync(25);
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
            _player.Queue.Shuffle();
        }

        public async Task Skip()
        {
            if (_player == null) throw new InvalidPlayerException();
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
            _player.Queue.Clear();
        }

        public IReadOnlyCollection<IPlayable> GetQueue()
        {
            return new List<IPlayable>(_player.Queue.Items.Cast<IPlayable>());
        }

        public async Task ChangeVolume(int volume)
        {
            if (_player == null) throw new InvalidPlayerException();
            if (!_player.IsPlaying) throw new InvalidPlayerException();
            await _player.SetVolumeAsync(volume);
        }
    }
}