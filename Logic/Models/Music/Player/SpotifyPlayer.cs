using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using IDal;
using Logic.Models.Music.Track;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;

namespace Logic.Models.Music.Player
{
    public class SpotifyPlayer : IMusicPlayer
    {
        private readonly IConfig _config;

        private SpotifyWebAPI _spotify;

        public SpotifyPlayer(IConfig config)
        {
            _config = config;
        }

        public bool IsPlaying { get; }
        public bool IsPaused { get; }
        public IVoiceChannel VoiceChannel { get; }
        public ITextChannel TextChannel { get; }

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

        public Task Play(IEnumerable<IPlayable> items)
        {
            throw new NotImplementedException();
        }

        public Task Stop()
        {
            throw new NotImplementedException();
        }

        public Task Skip(int amount = 1)
        {
            throw new NotImplementedException();
        }

        public Task Shuffle()
        {
            throw new NotImplementedException();
        }

        public Task Clear()
        {
            throw new NotImplementedException();
        }

        public Task Pause()
        {
            throw new NotImplementedException();
        }

        public Task UnPause()
        {
            throw new NotImplementedException();
        }

        public Task ChangeVolume(ushort value)
        {
            throw new NotImplementedException();
        }
    }
}
