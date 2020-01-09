using Discord;
using Discord.WebSocket;
using Logic.Exceptions;
using Logic.Models.Music.Search;
using Logic.Models.Music.Track;
using System;
using System.Linq;
using System.Threading.Tasks;
using Victoria;
using Victoria.Enums;

namespace Logic.Models.Music.Player
{
    public class VictoriaPlayer : IMusicPlayer
    {
        public event EventHandler<TrackEndedEventArgs> TrackEnded;
        public event EventHandler<TrackStuckEventArgs> TrackStuck;
        public event EventHandler<TrackExceptionEventArgs> TrackException;

        private readonly DiscordSocketClient _client;

        private readonly LavaConfig _lavaConfig;
        private readonly LavaNode _lavaNode;

        private LavaPlayer _player;


        public bool IsPlaying => _player != null && (_player.PlayerState == PlayerState.Playing || _player.PlayerState == PlayerState.Paused);
        public bool IsPaused => _player != null && _player.PlayerState == PlayerState.Paused;
        public PlayerState State => _player?.PlayerState ?? PlayerState.Disconnected;
        public IVoiceChannel VoiceChannel => _player?.VoiceChannel;
        public ITextChannel TextChannel => _player?.TextChannel;


        public VictoriaPlayer(DiscordSocketClient client)
        {
            _client = client;

            _lavaConfig = new LavaConfig();
            _lavaNode = new LavaNode(_client, _lavaConfig);

            _lavaNode.OnTrackEnded += OnTrackEnded;
            _lavaNode.OnTrackStuck += OnTrackStuck;
            _lavaNode.OnTrackException += OnTrackException;
        }

        private async Task OnTrackEnded(Victoria.EventArgs.TrackEndedEventArgs arg)
        {
            TrackEnded?.Invoke(this, new TrackEndedEventArgs(new VictoriaTrack(arg.Track), this, arg.Reason.ToString()));
        }

        private async Task OnTrackStuck(Victoria.EventArgs.TrackStuckEventArgs arg)
        {
            TrackStuck?.Invoke(this, new TrackStuckEventArgs(new VictoriaTrack(arg.Track), this, arg.Threshold));
        }

        private async Task OnTrackException(Victoria.EventArgs.TrackExceptionEventArgs arg)
        {
            TrackException?.Invoke(this, new TrackExceptionEventArgs(new VictoriaTrack(arg.Track), this, arg.ErrorMessage));
        }


        public async Task Ready()
        {
            if (_lavaNode.IsConnected) await _lavaNode.DisconnectAsync();
            await _lavaNode.ConnectAsync();
        }

        public async Task Prepare(IGuild guild)
        {
            _player = _lavaNode.GetPlayer(guild);
        }


        public async Task<SearchResult> Search(string query)
        {
            var result = await _lavaNode.SearchAsync(query);

            var tracks = result.Tracks.Select(x => new VictoriaTrack(x)).ToList();
            var resultStatus = ResultStatus.Failed;
            Playlist playlist = null;
            string exception = null;

            switch (result.LoadStatus)
            {
                case LoadStatus.LoadFailed:
                    resultStatus = ResultStatus.Failed;
                    exception = result.Exception.Message;
                    break;
                case LoadStatus.NoMatches:
                    resultStatus = ResultStatus.NoMatch;
                    break;
                case LoadStatus.SearchResult:
                    resultStatus = ResultStatus.SearchResult;
                    break;
                case LoadStatus.TrackLoaded:
                    resultStatus = ResultStatus.SingleTrack;
                    break;
                case LoadStatus.PlaylistLoaded:
                    resultStatus = ResultStatus.Playlist;
                    playlist = new Playlist(result.Playlist.Name, result.Playlist.SelectedTrack);
                    break;
            }

            return new SearchResult(tracks, resultStatus, playlist, exception);
        }


        public async Task Join(IVoiceChannel voiceChannel)
        {
            if (_lavaNode.HasPlayer(voiceChannel.Guild)) throw new InvalidPlayerException("Already connected to a voice channel.");

            await _lavaNode.JoinAsync(voiceChannel);
        }

        public async Task Move(IVoiceChannel voiceChannel)
        {
            if (!_lavaNode.HasPlayer(voiceChannel.Guild)) throw new InvalidPlayerException("Not connected to a voice channel");
            if (_player.VoiceChannel.Id.Equals(voiceChannel.Id)) throw new InvalidChannelException("The argument is the same as the source.");

            await _lavaNode.MoveAsync(voiceChannel);
        }

        public async Task Leave(IVoiceChannel voiceChannel)
        {
            if (!_lavaNode.TryGetPlayer(voiceChannel.Guild, out _player)) throw new InvalidPlayerException("Not connected to a voice channel");
            if (!_player.VoiceChannel.Id.Equals(voiceChannel.Id)) throw new InvalidChannelException("The argument is not the same as the source.");

            await _lavaNode.LeaveAsync(voiceChannel);
        }


        public async Task Play(IPlayable item)
        {
            if (_player == null) throw new InvalidPlayerException("There is no active player.");
            if (!(item.Track is VictoriaTrack track)) throw new InvalidTrackException("The requested track is not of the correct type.");
            await _player.PlayAsync(track.Track);
        }

        public async Task Stop()
        {
            if (_player == null) throw new InvalidPlayerException("There is no active player.");
            await _player.StopAsync();
        }


        public async Task Pause()
        {
            if (_player == null) throw new InvalidPlayerException("There is no active player.");
            if (_player.PlayerState == PlayerState.Paused) throw new InvalidOperationException("The player is already paused.");
            await _player.PauseAsync();
        }

        public async Task UnPause()
        {
            if (_player == null) throw new InvalidPlayerException("There is no active player.");
            if (_player.PlayerState != PlayerState.Paused) throw new InvalidOperationException("The player is not paused.");
            await _player.ResumeAsync();
        }


        public async Task ChangeVolume(ushort value)
        {
            if (_player == null) throw new InvalidPlayerException("There is no active player.");
            await _player.UpdateVolumeAsync(value);
        }
    }
}
