using Logic.Models.Music.Player;
using Logic.Models.Music.Track;
using System;

namespace Logic.Models.Music
{
    public class TrackEventArgs : EventArgs
    {
        public ITrack Track { get; private set; }
        public IMusicPlayer Player { get; private set; }

        public TrackEventArgs(ITrack track, IMusicPlayer player)
        {
            Track = track;
            Player = player;
        }
    }

    public class TrackExceptionEventArgs : TrackEventArgs
    {
        public string Message { get; private set; }

        public TrackExceptionEventArgs(ITrack track, IMusicPlayer player, string message) : base(track, player)
        {
            Message = message;
        }
    }

    public class TrackStuckEventArgs : TrackEventArgs
    {
        public TimeSpan Duration { get; private set; }

        public TrackStuckEventArgs(ITrack track, IMusicPlayer player, TimeSpan duration) : base(track, player)
        {
            Duration = duration;
        }
    }

    public class TrackEndedEventArgs : TrackEventArgs
    {
        public string Reason { get; private set; }

        public TrackEndedEventArgs(ITrack track, IMusicPlayer player, string reason) : base(track, player)
        {
            Reason = reason;
        }
    }
}