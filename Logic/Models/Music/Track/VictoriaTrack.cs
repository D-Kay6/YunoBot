using System;
using Victoria;

namespace Logic.Models.Music.Track
{
    public class VictoriaTrack : ITrack
    {
        public LavaTrack Track { get; }

        public string Title => Track.Title;
        public TimeSpan Duration => Track.Duration;

        public VictoriaTrack(LavaTrack track)
        {
            Track = track;
        }
    }
}