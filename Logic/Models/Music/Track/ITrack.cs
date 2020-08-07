using System;

namespace Logic.Models.Music.Track
{
    public interface ITrack
    {
        string Id { get; }
        string Title { get; }
        TimeSpan Duration { get; }
    }
}