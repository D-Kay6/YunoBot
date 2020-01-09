using Discord;
using Logic.Models.Music.Search;
using Logic.Models.Music.Track;
using System;
using System.Threading.Tasks;

namespace Logic.Models.Music.Player
{
    public interface IMusicPlayer
    {
        event EventHandler<TrackEndedEventArgs> TrackEnded;
        event EventHandler<TrackStuckEventArgs> TrackStuck;
        event EventHandler<TrackExceptionEventArgs> TrackException;

        bool IsPlaying { get; }
        bool IsPaused { get; }
        IVoiceChannel VoiceChannel { get; }
        ITextChannel TextChannel { get; }

        Task Ready();
        Task Prepare(IGuild guild);

        Task<SearchResult> Search(string query);

        Task Join(IVoiceChannel voiceChannel);
        Task Move(IVoiceChannel voiceChannel);
        Task Leave(IVoiceChannel voiceChannel);

        Task Play(IPlayable item);
        Task Stop();

        Task Pause();
        Task UnPause();

        Task ChangeVolume(ushort value);
    }
}