﻿using Discord;
using IDal;
using Logic.Models.Music.Event;
using Logic.Models.Music.Queue;
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
        private readonly IConfig _config;

        private SpotifyClient _spotify;

        public SpotifyPlayer(IConfig config)
        {
            _config = config;
        }

        public event Func<TrackEndedEventArgs, Task> TrackEnded;
        public event Func<TrackStuckEventArgs, Task> TrackStuck;
        public event Func<TrackExceptionEventArgs, Task> TrackException;
        public event Func<PlayerExceptionEventArgs, Task> PlayerException;

        public bool IsConnected { get; private set; }
        public bool IsPlaying { get; }
        public bool IsPaused { get; }
        public IVoiceChannel VoiceChannel { get; }
        public ITextChannel TextChannel { get; }
        public ITrack CurrentTrack { get; }


        public Task Initialize()
        {
            throw new NotImplementedException();
        }

        public Task Finish()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Connect to the player.
        /// </summary>
        public async Task Connect()
        {
            if (IsConnected) return;

            var configuration = await _config.Read();
            var auth = new ClientCredentialsAuthenticator(configuration.SpotifyId, configuration.SpotifySecret);
            if (auth.Token == null)
            {
                IsConnected = false;
                return;
            }
            _spotify = new SpotifyClient(auth.Token);
            IsConnected = true;
        }

        public Task Reconnect()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Disconnect from the player.
        /// </summary>
        public Task Disconnect()
        {
            if (IsConnected)
            {
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