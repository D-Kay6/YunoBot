using Discord;
using IDal;
using Logic.Models.Music.Search;
using Logic.Models.Music.Track;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using System;
using System.Threading.Tasks;

namespace Logic.Models.Music.Player
{
    public class SpotifyPlayer : IMusicPlayer
    {
        public event Func<TrackEndedEventArgs, Task> TrackEnded;
        public event Func<TrackStuckEventArgs, Task> TrackStuck;
        public event Func<TrackExceptionEventArgs, Task> TrackException;

        private readonly IConfig _config;

        private SpotifyWebAPI _spotify;

        public SpotifyPlayer(IConfig config)
        {
            _config = config;
        }

        public bool IsConnected { get; }
        public bool IsPlaying { get; }
        public bool IsPaused { get; }
        public IVoiceChannel VoiceChannel { get; }
        public ITextChannel TextChannel { get; }
        public ITrack CurrentTrack { get; }


        public async Task Ready()
        {
            var configuration = await _config.Read();
            var auth = new CredentialsAuth(configuration.SpotifyId, configuration.SpotifySecret);
            var token = await auth.GetToken();
            _spotify = new SpotifyWebAPI
            {
                AccessToken = token.AccessToken,
                TokenType = token.TokenType
            };
        }

        public Task Prepare(IGuild guild)
        {
            throw new NotImplementedException();
        }


        public Task<SearchResult> Search(string query)
        {
            throw new NotImplementedException();
        }


        public Task Join(IVoiceChannel voiceChannel)
        {
            throw new NotImplementedException();
        }

        public Task Move(IVoiceChannel voiceChannel)
        {
            throw new NotImplementedException();
        }

        public Task Leave(IVoiceChannel voiceChannel)
        {
            throw new NotImplementedException();
        }


        public Task Play(IPlayable item)
        {
            throw new NotImplementedException();
        }

        public Task Stop()
        {
            throw new NotImplementedException();
        }

        public Task Pause()
        {
            throw new NotImplementedException();
        }

        public Task Resume()
        {
            throw new NotImplementedException();
        }


        public Task ChangeVolume(ushort value)
        {
            throw new NotImplementedException();
        }
    }
}