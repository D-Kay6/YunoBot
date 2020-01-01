using Discord;
using Victoria;

namespace Logic.Services.Music
{
    public interface IPlayable
    {
        LavaTrack Track { get; }

        IGuild Guild { get; }

        IGuildUser Requester { get; }

        ITextChannel TextChannel { get; }

        string DurationString { get; }

        int Volume { get; }

        void OnPostPlay();
    }
}