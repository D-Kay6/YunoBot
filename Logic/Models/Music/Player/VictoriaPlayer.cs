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
        public event Func<TrackEndedEventArgs, Task> TrackEnded;
        public event Func<TrackStuckEventArgs, Task> TrackStuck;
        public event Func<TrackExceptionEventArgs, Task> TrackException;

        private readonly DiscordSocketClient _client;

        private readonly LavaConfig _lavaConfig;
        private readonly LavaNode _lavaNode;

        private LavaPlayer _player;


        public bool IsConnected => _player?.PlayerState == PlayerState.Connected || IsPlaying;
        public bool IsPlaying => _player?.PlayerState == PlayerState.Playing || _player?.PlayerState == PlayerState.Paused;
        public bool IsPaused => _player?.PlayerState == PlayerState.Paused;
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
            if (TrackEnded == null) return;
            await TrackEnded.Invoke(new TrackEndedEventArgs(new VictoriaTrack(arg.Track), this, arg.Reason.ToString()));
        }

        private async Task OnTrackStuck(Victoria.EventArgs.TrackStuckEventArgs arg)
        {
            if (TrackStuck == null) return;
            await TrackStuck?.Invoke(new TrackStuckEventArgs(new VictoriaTrack(arg.Track), this, arg.Threshold));
        }

        private async Task OnTrackException(Victoria.EventArgs.TrackExceptionEventArgs arg)
        {
            if (TrackException == null) return;
            await TrackException?.Invoke(new TrackExceptionEventArgs(new VictoriaTrack(arg.Track), this, arg.ErrorMessage));
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


        /// <summary>
        /// Search the source(s) for one or more tracks to play.
        /// </summary>
        /// <param name="query">The query to search for.</param>
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


        /// <summary>
        /// Connect to a voice channel.
        /// </summary>
        /// <param name="voiceChannel">The voice channel to connect to.</param>
        /// <exception cref="InvalidPlayerException">Thrown if already connected to a voice channel.</exception>
        public async Task Join(IVoiceChannel voiceChannel)
        {
            if (_lavaNode.HasPlayer(voiceChannel.Guild)) throw new InvalidPlayerException("Already connected to a voice channel.");

            await _lavaNode.JoinAsync(voiceChannel);
        }

        /// <summary>
        /// Move to a different voice channel.
        /// </summary>
        /// <param name="voiceChannel">The voice channel to connect to.</param>
        /// <exception cref="InvalidPlayerException">Thrown if not connected to a voice channel.</exception>
        /// <exception cref="InvalidChannelException">Thrown if already connected to the specified voice channel.</exception>
        public async Task Move(IVoiceChannel voiceChannel)
        {
            if (!_lavaNode.HasPlayer(voiceChannel.Guild)) throw new InvalidPlayerException("Not connected to a voice channel");
            if (_player.VoiceChannel.Id.Equals(voiceChannel.Id)) throw new InvalidChannelException("The argument is the same as the source.");

            await _lavaNode.MoveAsync(voiceChannel);
        }

        /// <summary>
        /// Disconnect from a voice channel.
        /// </summary>
        /// <param name="voiceChannel">The voice channel to connect to.</param>
        /// <exception cref="InvalidPlayerException">Thrown if not connected to a voice channel.</exception>
        /// <exception cref="InvalidChannelException">Thrown if not connected to the specified voice channel.</exception>
        public async Task Leave(IVoiceChannel voiceChannel)
        {
            if (!_lavaNode.TryGetPlayer(voiceChannel.Guild, out _player)) throw new InvalidPlayerException("Not connected to a voice channel");
            if (!_player.VoiceChannel.Id.Equals(voiceChannel.Id)) throw new InvalidChannelException("The argument is not the same as the source.");

            await _lavaNode.LeaveAsync(voiceChannel);
        }


        /// <summary>
        /// Play a track on the voice channel currently connected to.
        /// </summary>
        /// <param name="item">The track to play.</param>
        /// <exception cref="InvalidPlayerException">Thrown if not connected to a voice channel.</exception>
        /// <exception cref="InvalidTrackException">Thrown if the track is not of the correct type for the player.</exception>
        public async Task Play(IPlayable item)
        {
            if (_player == null) throw new InvalidPlayerException("There is no active player.");
            if (!(item.Track is VictoriaTrack track)) throw new InvalidTrackException("The requested track is not of the correct type.");
            await _player.PlayAsync(track.Track);
            _lavaNode.UpdateTextChannel(item.Guild, item.TextChannel);
        }

        /// <summary>
        /// Stop playing the current track.
        /// </summary>
        /// <exception cref="InvalidPlayerException">Thrown if not connected to a voice channel.</exception>
        /// <exception cref="InvalidTrackException">Thrown if there is no track playing.</exception>
        public async Task Stop()
        {
            if (_player == null) throw new InvalidPlayerException("There is no active player.");
            if (_player.Track == null) throw new InvalidTrackException("There is no track to stop playing.");
            await _player.StopAsync();
        }


        /// <summary>
        /// Pause playing the current track.
        /// </summary>
        /// <exception cref="InvalidPlayerException">Thrown if not connected to a voice channel.</exception>
        /// <exception cref="InvalidTrackException">Thrown if there is no track playing.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the player is already paused.</exception>
        public async Task Pause()
        {
            if (_player == null) throw new InvalidPlayerException("There is no active player.");
            if (_player.Track == null) throw new InvalidTrackException("There is no track playing.");
            if (_player.PlayerState == PlayerState.Paused) throw new InvalidOperationException("The player is already paused.");
            await _player.PauseAsync();
        }

        /// <summary>
        /// Resume playing the current track.
        /// </summary>
        /// <exception cref="InvalidPlayerException">Thrown if not connected to a voice channel.</exception>
        /// <exception cref="InvalidTrackException">Thrown if there is no track playing.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the player is not paused.</exception>
        public async Task Resume()
        {
            if (_player == null) throw new InvalidPlayerException("There is no active player.");
            if (_player.Track == null) throw new InvalidTrackException("There is no track playing.");
            if (_player.PlayerState != PlayerState.Paused) throw new InvalidOperationException("The player is not paused.");
            await _player.ResumeAsync();
        }


        /// <summary>
        /// Change the volume of the player.
        /// </summary>
        /// <param name="value">The value to set the volume to. Range: 0 - 150</param>
        /// <exception cref="InvalidPlayerException">Thrown if not connected to a voice channel.</exception>
        public async Task ChangeVolume(ushort value)
        {
            if (_player == null) throw new InvalidPlayerException("There is no active player.");
            await _player.UpdateVolumeAsync(value);
        }
    }
}