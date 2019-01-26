using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Yuno.Main.Extentions;
using Yuno.Main.Logging;

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

        private CancellationTokenSource _token;
        
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

        public async Task Stop(string reason = null)
        {
            if (_token == null) return;
            _songQueue.Clear();
            if (VoiceChannel != null)
            {
                var msg = "The music player was terminated.";
                if (reason != null) msg += $" {reason}";
                await NowPlaying.TextChannel.SendMessageAsync(msg);
            }
            _token.Cancel();
            //AudioPlaybackService.StopCurrentOperation();
        }

        public void Next()
        {
            _token.Cancel();
            //AudioPlaybackService.StopCurrentOperation();
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
        
        private async Task ProcessQueue()
        {
            try
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
                            _token = new CancellationTokenSource();
                            await NowPlaying.TextChannel.SendMessageAsync(
                                $"Now playing **{NowPlaying.Title}** (`{NowPlaying.DurationString}`) | requested by {NowPlaying.Requester.Username}");
                            await AudioPlaybackService.SendAsync(audioClient, NowPlaying.Uri, NowPlaying.Volume,
                                NowPlaying.Speed, _token);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($"Error while playing song: {e}");
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogsHandler.Instance.Log("Crashes", $"Exception occured in SongService. Traceback: {e}");
            }
            finally
            {
                IsPlaying = false;
                VoiceChannel = null;
            }
        }
    }
}
