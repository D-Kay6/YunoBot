namespace Logic.Models.Music.Track
{
    using System;
    using Victoria;

    public class VictoriaTrack : ITrack
    {
        public VictoriaTrack(LavaTrack track)
        {
            Track = track;
        }

        public LavaTrack Track { get; }

        public string Id => Track.Id;
        public string Title => Track.Title;
        public TimeSpan Duration => Track.Duration;
    }
}