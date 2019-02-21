using Discord;
using Victoria.Entities;

namespace Logic.Services
{
    public interface IPlayable
    {
        LavaTrack Track { get; }

        IGuild Guild { get; }

        IGuildUser Requester { get; }

        IMessageChannel TextChannel { get; }

        string DurationString { get; }

        int Volume { get; }

        void OnPostPlay();
    }
}