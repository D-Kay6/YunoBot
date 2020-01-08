using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Victoria;

namespace Logic.Models.Music
{
    public class Song : IPlayable
    {
        public Song(LavaTrack track, SocketCommandContext context, int volume = 25)
        {
            Track = track;
            Guild = context.Guild;
            Requester = (SocketGuildUser) context.User;
            TextChannel = context.Channel as ITextChannel;
            DurationString = track.Duration.ToString();
            Volume = volume;
        }

        public string Id => Track.Id;

        public LavaTrack Track { get; }

        public IGuild Guild { get; }

        public IGuildUser Requester { get; }

        public ITextChannel TextChannel { get; }

        public string DurationString { get; }

        public int Volume { get; }
    }
}