using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Victoria.Entities;

namespace Yuno.Main.Music
{
    public class Song : IPlayable
    {
        public LavaTrack Track { get; }

        public IGuild Guild { get; }

        public IGuildUser Requester { get; }

        public IMessageChannel TextChannel { get; }

        public string DurationString { get; }

        public int Volume { get; }

        public Song(LavaTrack track, SocketCommandContext context, int volume)
        {
            Track = track;
            Guild = context.Guild;
            Requester = (SocketGuildUser)context.User;
            TextChannel = context.Channel;
            DurationString = track.Length.ToString();
            Volume = volume;
        }

        public void OnPostPlay()
        {
            // do nothing;
        }
    }
}
