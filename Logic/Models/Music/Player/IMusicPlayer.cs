using Discord;
using Logic.Exceptions;
using Logic.Models.Music.Search;
using Logic.Models.Music.Track;
using System;
using System.Threading.Tasks;

namespace Logic.Models.Music.Player
{
    public interface IMusicPlayer : IPlayer
    {
        bool IsConnected { get; }
        bool IsPlaying { get; }
        bool IsPaused { get; }
        ITrack CurrentTrack { get; }

        Task Prepare(IGuild guild);

        /// <summary>
        /// Search the source(s) for one or more tracks to play.
        /// </summary>
        /// <param name="query">The query to search for.</param>
        Task<SearchResult> Search(string query);

        /// <summary>
        /// Connect to a voice channel.
        /// </summary>
        /// <param name="voiceChannel">The voice channel to connect to.</param>
        /// <exception cref="InvalidPlayerException">Thrown if already connected to a voice channel.</exception>
        Task Join(IVoiceChannel voiceChannel);
        /// <summary>
        /// Move to a different voice channel.
        /// </summary>
        /// <param name="voiceChannel">The voice channel to connect to.</param>
        /// <exception cref="InvalidPlayerException">Thrown if not connected to a voice channel.</exception>
        /// <exception cref="InvalidChannelException">Thrown if already connected to the specified voice channel.</exception>
        Task Move(IVoiceChannel voiceChannel);
        /// <summary>
        /// Disconnect from a voice channel.
        /// </summary>
        /// <param name="voiceChannel">The voice channel to connect to.</param>
        /// <exception cref="InvalidPlayerException">Thrown if not connected to a voice channel.</exception>
        /// <exception cref="InvalidChannelException">Thrown if not connected to the specified voice channel.</exception>
        Task Leave(IVoiceChannel voiceChannel);

        /// <summary>
        /// Play a track on the voice channel currently connected to.
        /// </summary>
        /// <param name="item">The track to play.</param>
        /// <exception cref="InvalidPlayerException">Thrown if not connected to a voice channel.</exception>
        /// <exception cref="InvalidFormatException">Thrown if the track is not of the correct type for the player.</exception>
        Task Play(IPlayable item);
        /// <summary>
        /// Stop playing the current track.
        /// </summary>
        /// <exception cref="InvalidPlayerException">Thrown if not connected to a voice channel.</exception>
        /// <exception cref="InvalidTrackException">Thrown if there is no track playing.</exception>
        Task Stop();

        /// <summary>
        /// Pause playing the current track.
        /// </summary>
        /// <exception cref="InvalidPlayerException">Thrown if not connected to a voice channel.</exception>
        /// <exception cref="InvalidTrackException">Thrown if there is no track playing.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the player is already paused.</exception>
        Task Pause();
        /// <summary>
        /// Resume playing the current track.
        /// </summary>
        /// <exception cref="InvalidPlayerException">Thrown if not connected to a voice channel.</exception>
        /// <exception cref="InvalidTrackException">Thrown if there is no track playing.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the player is not paused.</exception>
        Task Resume();

        /// <summary>
        /// Change the volume of the player.
        /// </summary>
        /// <param name="value">The value to set the volume to. Range: 0 - 150</param>
        /// <exception cref="InvalidPlayerException">Thrown if not connected to a voice channel.</exception>
        Task ChangeVolume(ushort value);
    }
}