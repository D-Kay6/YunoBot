using System;
using Victoria;

namespace Logic.Models.Music.Track
{
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