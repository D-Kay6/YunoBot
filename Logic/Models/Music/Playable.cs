using Discord;
using Logic.Models.Music.Track;

namespace Logic.Models.Music
{
    public class Playable : IPlayable
    {
        public ITrack Track { get; }

        public IGuild Guild { get; }

        public IGuildUser Requester { get; }

        public ITextChannel TextChannel { get; }

        public int Volume { get; }

        public Playable(ITrack track, IGuildUser requester, ITextChannel textChannel, int volume = 25)
        {
            Track = track;
            Guild = requester.Guild;
            Requester = requester;
            TextChannel = textChannel;
            Volume = volume;
        }
    }
}