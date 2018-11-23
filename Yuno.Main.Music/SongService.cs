using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Yuno.Main.Extentions;

namespace Yuno.Main.Music
{
    public class SongService
    {
        private static Dictionary<ulong, SongService> _songServices = new Dictionary<ulong, SongService>();

        public static SongService GetSongService(ulong id)
        {
            if (_songServices.ContainsKey(id)) return _songServices[id];
            var songService = new SongService();
            _songServices.Add(id, songService);
            return songService;
        }


        public AudioPlaybackService AudioPlaybackService { get; set; }

        public IVoiceChannel VoiceChannel { get; private set; }

        public IPlayable NowPlaying { get; private set; }

        public bool IsPlaying { get; private set; }

        private Queue<IPlayable> _songQueue;


        public SongService()
        {
            _songQueue = new Queue<IPlayable>();
            AudioPlaybackService = new AudioPlaybackService();
        }


        public void SetVoiceChannel(IVoiceChannel voiceChannel)
        {
            this.VoiceChannel = voiceChannel;
        }

        public IEnumerable<IPlayable> GetQueue()
        {
            return _songQueue;
        }

        public void Next()
        {
            AudioPlaybackService.StopCurrentOperation();
        }

        public void Shuffle()
        {
            lock (_songQueue)
            {
                _songQueue = _songQueue.Shuffle();
            }
        }

        public IList<IPlayable> Clear()
        {
            var skippedSongs = _songQueue.ToList();
            _songQueue.Clear();

            Console.WriteLine($"Skipped {skippedSongs.Count} songs");

            return skippedSongs;
        }

        public int Queue(IPlayable video)
        {
            _songQueue.Enqueue(video);
            if (!IsPlaying) ProcessQueue();
            return _songQueue.Count;
        }


        private async void ProcessQueue()
        {
            IsPlaying = true;
            Console.WriteLine("Connecting to voice channel");
            using (var audioClient = await VoiceChannel.ConnectAsync())
            {
                Console.WriteLine("Connected!");
                while (_songQueue.Count > 0)
                {
                    Console.WriteLine("Waiting for songs");
                    NowPlaying = _songQueue.Dequeue();
                    try
                    {
                        await NowPlaying.TextChannel.SendMessageAsync($"Now playing **{NowPlaying.Title}** (`{NowPlaying.DurationString}`) | requested by {NowPlaying.Requester.Mention}");
                        await AudioPlaybackService.SendAsync(audioClient, NowPlaying.Uri, NowPlaying.Volume, NowPlaying.Speed);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error while playing song: {e}");
                    }
                }
            }
            IsPlaying = false;
        }
    }
}
