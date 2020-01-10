using Discord;
using System;
using System.Threading.Tasks;

namespace Logic.Models.Music.Player
{
    public interface IPlayer
    {
        event Func<TrackEndedEventArgs, Task> TrackEnded;
        event Func<TrackStuckEventArgs, Task> TrackStuck;
        event Func<TrackExceptionEventArgs, Task> TrackException;

        Task Ready();

        IVoiceChannel VoiceChannel { get; }
        ITextChannel TextChannel { get; }
    }
}