using Discord;
using Logic.Models.Music.Track;

namespace Logic.Models.Music.Queue
{
    public interface IPlayable
    {
        ITrack Track { get; }
        IGuild Guild { get; }
        IGuildUser Requester { get; }
        ITextChannel TextChannel { get; }
        int Volume { get; }
    }
}