namespace Logic.Models.Music.Player
{
    using System;
    using System.Threading.Tasks;
    using Discord;

    public interface IPlayer
    {
        IVoiceChannel VoiceChannel { get; }
        ITextChannel TextChannel { get; }
        event Func<TrackEndedEventArgs, Task> TrackEnded;
        event Func<TrackStuckEventArgs, Task> TrackStuck;
        event Func<TrackExceptionEventArgs, Task> TrackException;

        Task Ready();
    }
}