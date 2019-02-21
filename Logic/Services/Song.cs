using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Victoria.Entities;

namespace Logic.Services
{
    public class Song : IPlayable
    {
        public Song(LavaTrack track, SocketCommandContext context, int volume)
        {
            Track = track;
            Guild = context.Guild;
            Requester = (SocketGuildUser) context.User;
            TextChannel = context.Channel;
            DurationString = track.Length.ToString();
            Volume = volume;
        }

        public LavaTrack Track { get; }

        public IGuild Guild { get; }

        public IGuildUser Requester { get; }

        public IMessageChannel TextChannel { get; }

        public string DurationString { get; }

        public int Volume { get; }

        public void OnPostPlay()
        {
            // do nothing;
        }
    }
}