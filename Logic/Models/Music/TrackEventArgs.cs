namespace Logic.Models.Music
{
    using System;
    using Player;
    using Track;

    public class TrackEventArgs : EventArgs
    {
        public TrackEventArgs(ITrack track, IMusicPlayer player)
        {
            Track = track;
            Player = player;
        }

        public ITrack Track { get; }
        public IMusicPlayer Player { get; }
    }

    public class TrackExceptionEventArgs : TrackEventArgs
    {
        public TrackExceptionEventArgs(ITrack track, IMusicPlayer player, string message) : base(track, player)
        {
            Message = message;
        }

        public string Message { get; }
    }

    public class TrackStuckEventArgs : TrackEventArgs
    {
        public TrackStuckEventArgs(ITrack track, IMusicPlayer player, TimeSpan duration) : base(track, player)
        {
            Duration = duration;
        }

        public TimeSpan Duration { get; }
    }

    public class TrackEndedEventArgs : TrackEventArgs
    {
        public TrackEndedEventArgs(ITrack track, IMusicPlayer player, string reason) : base(track, player)
        {
            Reason = reason;
        }

        public string Reason { get; }
    }
}