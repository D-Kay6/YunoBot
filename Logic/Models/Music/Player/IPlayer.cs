using Discord;
using Logic.Models.Music.Event;
using System;
using System.Threading.Tasks;

namespace Logic.Models.Music.Player
{
    public interface IPlayer
    {
        IVoiceChannel VoiceChannel { get; }
        ITextChannel TextChannel { get; }
        event Func<TrackEndedEventArgs, Task> TrackEnded;
        event Func<TrackStuckEventArgs, Task> TrackStuck;
        event Func<TrackExceptionEventArgs, Task> TrackException;

        event Func<PlayerExceptionEventArgs, Task> PlayerException;

        /// <summary>
        ///     Start the player.
        /// </summary>\
        Task Initialize();

        /// <summary>
        ///     Clean up and close the player.
        /// </summary>\
        Task Finish();

        /// <summary>
        ///     Connect to the player.
        /// </summary>
        Task Connect();

        /// <summary>
        ///     Reconnect to the player.
        /// </summary>
        Task Reconnect();

        /// <summary>
        ///     Disconnect from the player.
        /// </summary>
        Task Disconnect();
    }
}