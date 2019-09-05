using Discord;
using Victoria.Entities;
using Victoria.Queue;

namespace Logic.Services.Music
{
    public interface IPlayable : IQueueObject
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