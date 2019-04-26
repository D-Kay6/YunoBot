using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Victoria.Entities;
using Victoria.Queue;

namespace Logic.Services
{
    public class Song : IPlayable
    {
        public Song(LavaTrack track, SocketCommandContext context, int volume = 25)
        {
            Track = track;
            Guild = context.Guild;
            Requester = (SocketGuildUser) context.User;
            TextChannel = context.Channel as ITextChannel;
            DurationString = track.Length.ToString();
            Volume = volume;
        }

        public string Id => Track.Id;

        public LavaTrack Track { get; }

        public IGuild Guild { get; }

        public IGuildUser Requester { get; }

        public ITextChannel TextChannel { get; }

        public string DurationString { get; }

        public int Volume { get; }

        public void OnPostPlay()
        {
            // do nothing;
        }
    }
}