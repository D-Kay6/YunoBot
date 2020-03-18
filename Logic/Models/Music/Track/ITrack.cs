namespace Logic.Models.Music.Track
{
    using System;

    public interface ITrack
    {
        string Id { get; }
        string Title { get; }
        TimeSpan Duration { get; }
    }
}