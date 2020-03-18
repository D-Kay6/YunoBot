﻿namespace Logic.Models.Music
{
    using Discord;
    using Track;

    public class Playable : IPlayable
    {
        public Playable(ITrack track, IGuildUser requester, ITextChannel textChannel, int volume = 25)
        {
            Track = track;
            Guild = requester.Guild;
            Requester = requester;
            TextChannel = textChannel;
            Volume = volume;
        }

        public ITrack Track { get; }

        public IGuild Guild { get; }

        public IGuildUser Requester { get; }

        public ITextChannel TextChannel { get; }

        public int Volume { get; }
    }
}