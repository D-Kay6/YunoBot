namespace Logic.Models.Music.Player
{
    using System;
    using System.Threading.Tasks;
    using Discord;
    using IDal;
    using Search;
    using SpotifyAPI.Web;
    using SpotifyAPI.Web.Auth;
    using Track;

    public class SpotifyPlayer : IMusicPlayer
    {
        private readonly IConfig _config;

        private SpotifyWebAPI _spotify;

        public SpotifyPlayer(IConfig config)
        {
            _config = config;
        }

        public event Func<TrackEndedEventArgs, Task> TrackEnded;
        public event Func<TrackStuckEventArgs, Task> TrackStuck;
        public event Func<TrackExceptionEventArgs, Task> TrackException;

        public bool IsConnected { get; private set; }
        public bool IsPlaying { get; }
        public bool IsPaused { get; }
        public IVoiceChannel VoiceChannel { get; }
        public ITextChannel TextChannel { get; }
        public ITrack CurrentTrack { get; }


        /// <summary>
        ///     Connect to the player.
        /// </summary>
        public async Task Connect()
        {
            if (IsConnected) return;

            var configuration = await _config.Read();
            var auth = new CredentialsAuth(configuration.SpotifyId, configuration.SpotifySecret);
            var token = await auth.GetToken();
            _spotify = new SpotifyWebAPI
            {
                AccessToken = token.AccessToken,
                TokenType = token.TokenType
            };
            IsConnected = true;
        }

        /// <summary>
        ///     Disconnect from the player.
        /// </summary>
        public Task Disconnect()
        {
            if (IsConnected)
            {
                _spotify.Dispose();
                _spotify = null;
                IsConnected = false;
            }

            return Task.CompletedTask;
        }

        /// <summary>
        ///     Prepare the music player to work for the selected guild.
        /// </summary>
        /// <param name="guild">The guild to load the music player for.</param>
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