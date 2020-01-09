﻿using Discord;
using Discord.WebSocket;
using IDal.Database;
using Logic.Exceptions;
using Logic.Extensions;
using Logic.Models.Music;
using Logic.Models.Music.Player;
using Logic.Models.Music.Search;
using Logic.Models.Music.Track;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Victoria;
using Victoria.Enums;
using Victoria.Responses.Rest;

namespace Logic.Services
{
    public class MusicService
    {
        private readonly IMusicPlayer _player;

        private readonly DiscordSocketClient _client;
        private readonly Queue _queue;

        private IDbLanguage _language;
        private LocalizationService _localization;


        public MusicService(DiscordSocketClient client, IDbLanguage language, LocalizationService localization)
        {
            _client = client;
            _player = new VictoriaPlayer(client);
            _queue = new Queue();

            _language = language;
            _localization = localization;
        }


        public async Task<SearchResult> Search(string query)
        {
            return await _player.Search(query);
        }


        public async Task Prepare(IGuild guild)
        {
            await _player.Prepare(guild);
            await _localization.Load(await _language.GetLanguage(guild.Id));
        }


        public async Task PlayNextTrack()
        {
            if (_queue.Count == 0)
            {
                await EndPlayer();
                return;
            }

            var song = _queue.Dequeue();
            LavaNode.UpdateTextChannel(song.Guild, song.TextChannel);
            await song.TextChannel.SendMessageAsync(_localization.GetMessage("Music now playing", song.Track.Title, song.Requester.Nickname()));
            try
            {
                await _player.PlayAsync(song.Track);
            }
            catch (Exception e)
            {

            }
        }

        public async Task EndPlayer()
        {
            if (_player == null) throw new InvalidPlayerException();
            _queue.Clear();
            await LavaNode.LeaveAsync(_player.VoiceChannel);

        }



        public async Task Queue(IPlayable item)
        {
            _queue.Enqueue(item);
            if (_player == null) _player = await LavaNode.JoinAsync(song.Requester.VoiceChannel);
            _queue.Enqueue(song);
            if (_player.PlayerState == PlayerState.Connected)
            {
                await PlayNextTrack();
                await _player.UpdateVolumeAsync(25);
                return;
            }
            await song.TextChannel.SendMessageAsync(_localization.GetMessage("Music queued song", _queue.Count, song.Track.Title, song.Track.Duration));
        }

        public async Task Queue(IEnumerable<IPlayable> songs, IVoiceChannel channel)
        {
            if (IsPlaying)
            {
                songs.Foreach(s => _queue.Enqueue(s));
                return;
            }

            _player = await LavaNode.JoinAsync(channel);
            songs.Foreach(s => _queue.Enqueue(s));
            await PlayNextTrack();
            await _player.UpdateVolumeAsync(25);
        }

        public async Task<bool> Play()
        {
            if (_player == null) throw new InvalidPlayerException();
            if (_player.PlayerState != PlayerState.Paused) return false;
            await _player.ResumeAsync();
            return true;
        }

        public async Task<bool> Pause()
        {
            if (_player == null) throw new InvalidPlayerException();
            if (_player.PlayerState == PlayerState.Paused) return false;
            await _player.PauseAsync();
            return true;
        }

        public void Shuffle()
        {
            _queue.Shuffle();
        }

        public async Task Skip(int amount = 1)
        {
            if (_player == null) throw new InvalidPlayerException();

            for (var i = 0; i < amount - 1; i++) _queue.Dequeue();

            await PlayNextTrack();
        }

        public async Task Stop(string type = "command")
        {
            if (_player == null) throw new InvalidPlayerException();
            await TextChannel.SendMessageAsync(_localization.GetMessage($"Music stop {type}"));
            await _player.StopAsync();
        }

        public void Clear()
        {
            if (_player == null) throw new InvalidPlayerException();
            _queue.Clear();
        }

        public IReadOnlyCollection<IPlayable> GetQueue()
        {
            return _queue.ToList();
        }

        public async Task ChangeVolume(ushort volume)
        {
            if (_player == null) throw new InvalidPlayerException();
            if (_player.PlayerState != PlayerState.Playing) throw new InvalidPlayerException();
            await _player.UpdateVolumeAsync(volume);
        }
    }
}