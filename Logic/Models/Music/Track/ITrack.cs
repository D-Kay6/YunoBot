using System;

namespace Logic.Models.Music.Track
{
    public interface ITrack
    {
        string Title { get; }
        TimeSpan Duration { get; }
    }
}
