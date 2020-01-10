using Discord;
using Discord.WebSocket;
using Logic.Exceptions;
using Logic.Models.Music;
using Logic.Models.Music.Player;
using Logic.Models.Music.Search;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class MusicService
    {
        private readonly DiscordSocketClient _client;

        private readonly IMusicPlayer _player;
        private readonly Queue _queue;

        private IGuild _guild;

        public IPlayer Player => _player;

        public MusicService(DiscordSocketClient client)
        {
            _client = client;

            _player = new VictoriaPlayer(client);
            _queue = new Queue();
        }

        public async Task Prepare(IGuild guild)
        {
            _guild = guild;
            await _player.Prepare(guild);
        }


        /// <summary>
        /// Search for a track or playlist.
        /// </summary>
        /// <param name="query">The value to search for</param>
        /// <returns>Returns a SearchResult containing the track(s) found.</returns>
        public async Task<SearchResult> Search(string query)
        {
            return await _player.Search(query);
        }


        /// <summary>
        /// Connect to a voice channel.
        /// </summary>
        /// <param name="channel">The voice channel to connect to.</param>
        /// <exception cref="InvalidPlayerException">Thrown if already connected to a voice channel.</exception>
        public async Task Join(IVoiceChannel channel)
        {
            await _player.Join(channel);
        }

        /// <summary>
        /// Move to a different voice channel.
        /// </summary>
        /// <param name="channel">The voice channel to connect to.</param>
        /// <exception cref="InvalidPlayerException">Thrown if not connected to a voice channel.</exception>
        /// <exception cref="InvalidChannelException">Thrown if already connected to the specified voice channel.</exception>
        public async Task Move(IVoiceChannel channel)
        {
            await _player.Move(channel);
        }

        /// <summary>
        /// Disconnect from the voice channel.
        /// </summary>
        /// <exception cref="InvalidPlayerException">Thrown if not connected to a voice channel.</exception>
        public async Task Leave()
        {
            var channel = _player.VoiceChannel;
            await _player.Leave(channel);
        }


        /// <summary>
        /// Play the next track in the queue. 
        /// </summary>
        /// <returns>The track that will be played.</returns>
        /// <exception cref="InvalidTrackException">Thrown if the track is not of the correct type for the player.</exception>
        public async Task<IPlayable> PlayNext()
        {
            //await song.TextChannel.SendMessageAsync(_localization.GetMessage("Music now playing", song.Track.Title, song.Requester.Nickname()));
            IPlayable track;
            try
            {
                track = _queue.Dequeue(_guild);
            }
            catch (InvalidOperationException)
            {
                await Stop();
                return null;
            }

            if (!_player.IsConnected)
            {
                await _player.Join(track.Requester.VoiceChannel);
            }

            await _player.Play(track).ConfigureAwait(false);
            return track;
        }

        /// <summary>
        /// Pause playing the current track.
        /// </summary>
        /// <exception cref="InvalidPlayerException">Thrown if not connected to a voice channel.</exception>
        /// <exception cref="InvalidTrackException">Thrown if there is no track playing.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the player is already paused.</exception>
        public async Task Pause()
        {
            await _player.Pause();
        }

        /// <summary>
        /// Resume playing the current track.
        /// </summary>
        /// <exception cref="InvalidPlayerException">Thrown if not connected to a voice channel.</exception>
        /// <exception cref="InvalidTrackException">Thrown if there is no track playing.</exception>
        /// <exception cref="InvalidOperationException">Thrown if the player is not paused.</exception>
        public async Task Resume()
        {
            await _player.Resume();
        }

        /// <summary>
        /// Stop playing the current track.
        /// Remove any tracks remaining in the queue.
        /// Disconnects from the voice channel.
        /// </summary>
        /// <exception cref="InvalidPlayerException">Thrown if not connected to a voice channel.</exception>
        /// <exception cref="InvalidTrackException">Thrown if there is no track playing.</exception>
        public async Task Stop()
        {
            _queue.Clear(_guild);
            await _player.Stop();
            await _player.Leave(_player.VoiceChannel);
        }


        /// <summary>
        /// Add a track to the queue of a server.
        /// </summary>
        /// <param name="item">The track to add to the queue.</param>
        /// <returns>The index of the track that was added.</returns>
        public int Queue(IPlayable item)
        {
            return _queue.Enqueue(item);
            //await song.TextChannel.SendMessageAsync(_localization.GetMessage("Music queued song", _queue.Count, song.Track.Title, song.Track.Duration));
        }

        /// <summary>
        /// Add a list of tracks to the queue of a server. The server is determined by the first track in the list.
        /// </summary>
        /// <param name="items">The tracks to add to the queue.</param>
        public void Queue(IEnumerable<IPlayable> items)
        {
            _queue.Enqueue(items);
        }

        /// <summary>
        /// Get the queue for the server.
        /// </summary>
        /// <returns>The list of tracks for the server.</returns>
        public IReadOnlyCollection<IPlayable> GetQueue()
        {
            return _queue.GetItems(_guild);
        }


        /// <summary>
        /// Shuffle the tracks in the queue.
        /// </summary>
        public void Shuffle()
        {
            _queue.Shuffle(_guild);
        }

        /// <summary>
        /// Skip one or more tracks and play the next track in the queue.
        /// </summary>
        /// <param name="amount">The amount of tracks to skip</param>
        /// <exception cref="InvalidTrackException">Thrown if the track is not of the correct type for the player.</exception>
        /// <returns>The track that will be played.</returns>
        public async Task<IPlayable> Skip(int amount = 1)
        {
            if (amount > 1) _queue.Remove(_guild, amount - 1);
            return await PlayNext();
        }

        /// <summary>
        /// Remove the remaining tracks in the queue.
        /// </summary>
        public void Clear()
        {
            _queue.Clear(_guild);
        }


        /// <summary>
        /// Change the volume of the player.
        /// </summary>
        /// <param name="volume">The new volume. Min: 0, Max: 150</param>
        /// <exception cref="InvalidPlayerException">Thrown if not connected to a voice channel.</exception>
        public async Task ChangeVolume(ushort volume)
        {
            await _player.ChangeVolume(volume);
        }
    }
}