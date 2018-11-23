using Discord;

namespace Yuno.Main.Music
{
    public interface IPlayable
    {
        string Url { get; set; }

        string Uri { get; }

        string Title { get; }

        IUser Requester { get; set; }

        IMessageChannel TextChannel { get; }

        string DurationString { get; }

        int Volume { get; }

        int Speed { get; }

        void OnPostPlay();
    }
}
