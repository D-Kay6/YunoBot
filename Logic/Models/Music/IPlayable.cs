namespace Logic.Models.Music
{
    using Discord;
    using Track;

    public interface IPlayable
    {
        ITrack Track { get; }
        IGuild Guild { get; }
        IGuildUser Requester { get; }
        ITextChannel TextChannel { get; }
        int Volume { get; }
    }
}