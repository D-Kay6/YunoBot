using Discord;
using Victoria;

namespace Logic.Models.Music
{
    public interface IPlayable
    {
        LavaTrack Track { get; }

        IGuild Guild { get; }

        IGuildUser Requester { get; }

        ITextChannel TextChannel { get; }

        string DurationString { get; }

        int Volume { get; }
    }
}