using System;
using System.Collections.Generic;
using System.Threading.Tasks.Dataflow;
using Discord;

namespace Yuno.Main.Music
{
    public class SongService
    {
        public IVoiceChannel VoiceChannel { get; private set; }
        public IMessageChannel MessageChannel { get; private set; }

        private BufferBlock<IPlayable> _songQueue;

        public SongService()
        {
            _songQueue = new BufferBlock<IPlayable>();
        }

        public AudioPlaybackService AudioPlaybackService { get; set; }

        public IPlayable NowPlaying { get; private set; }

        public void SetVoiceChannel(IVoiceChannel voiceChannel)
        {
            this.VoiceChannel = voiceChannel;
            ProcessQueue();
        }

        public void SetMessageChannel(IMessageChannel messageChannel)
        {
            this.MessageChannel = messageChannel;
        }

        public void Next()
        {
            AudioPlaybackService.StopCurrentOperation();
        }

        public IList<IPlayable> Clear()
        {
            _songQueue.TryReceiveAll(out var skippedSongs);

            Console.WriteLine($"Skipped {skippedSongs.Count} songs");

            return skippedSongs;
        }

        public void Queue(IPlayable video)
        {
            _songQueue.Post(video);
        }

        private async void ProcessQueue()
        {
            while (await _songQueue.OutputAvailableAsync())
            {
                Console.WriteLine("Waiting for songs");
                NowPlaying = await _songQueue.ReceiveAsync();
                try
                {
                    await MessageChannel?.SendMessageAsync($"Now playing **{NowPlaying.Title}** | `{NowPlaying.DurationString}` | requested by {NowPlaying.Requester} | {NowPlaying.Url}");

                    Console.WriteLine("Connecting to voice channel");
                    using (var audioClient = await VoiceChannel.ConnectAsync())
                    {
                        Console.WriteLine("Connected!");
                        await AudioPlaybackService.SendAsync(audioClient, NowPlaying.Uri, NowPlaying.Speed);
                    }

                    NowPlaying.OnPostPlay();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Error while playing song: {e}");
                }
            }
        }
    }
}
