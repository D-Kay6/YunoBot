using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Yuno.Main.Extentions;
using Yuno.Main.Music;
using Yuno.Main.Music.YouTube;

namespace Yuno.Main.Commands.Modules
{
    [Group("music")]
    public class MusicModule : ModuleBase<SocketCommandContext>
    {
        public YouTubeDownloadService YoutubeDownloadService { get; set; }

        private SongService _songService;

        [Priority(-1)]
        [Command]
        public async Task DefaultMusic()
        {
            await MusicPlaying();
        }

        [Alias("request")]
        [Command("play")]
        [Summary("Requests a song to be played")]
        public async Task MusicPlay([Remainder]string url)
        {
            _songService = SongService.GetSongService(Context.Guild.Id);
            if (!await CanPerform()) return;
            _songService.SetVoiceChannel(((SocketGuildUser)Context.User).VoiceChannel);
            await Play(url);
        }

        [Command("shuffle")]
        public async Task MusicShuffle()
        {
            _songService = SongService.GetSongService(Context.Guild.Id);
            var notice = await ReplyAsync("Shuffling...");
            _songService.Shuffle();
            await notice.DeleteAsync();
            await ReplyAsync("The queue has been shuffled.");
        }

        [Command("queue")]
        public async Task MusicQueue()
        {
            _songService = SongService.GetSongService(Context.Guild.Id);
            var queue = _songService.GetQueue();
            if (!queue.Any())
            {
                await ReplyAsync("There is no music in the queue. Use /music play to add some music.");
                return;
            }
            for (var i = 1; i <= queue.Count(); i++)
            {
                var video = queue.ElementAt(i - 1);
                await ReplyAsync($"#{i}: **{video.Title}**");
            }
        }

        [Command("skip")]
        public async Task MusicSkip()
        {
            _songService = SongService.GetSongService(Context.Guild.Id);
            if (!await CanPerform()) return;
            _songService.Next();
            await ReplyAsync("Skipped song");
        }

        [Command("clear")]
        public async Task MusicClear()
        {
            _songService = SongService.GetSongService(Context.Guild.Id);
            if (!await CanPerform()) return;
            _songService.Clear();
            await ReplyAsync("Queue cleared");
        }

        [Command("stop")]
        public async Task MusicStop()
        {
            _songService = SongService.GetSongService(Context.Guild.Id);
            if (!await CanPerform()) return;
            _songService.Stop();
        }

        [Alias("now")]
        [Command("playing")]
        public async Task MusicPlaying()
        {
            _songService = SongService.GetSongService(Context.Guild.Id);
            if (_songService.NowPlaying == null)
            {
                await ReplyAsync($"{Context.User.Mention} current queue is empty");
            }
            else
            {
                await ReplyAsync($"{Context.User.Mention} now playing `{_songService.NowPlaying.Title}` requested by {_songService.NowPlaying.Requester.Username}");
            }
        }

        private async Task<bool> CanPerform()
        {
            var user = Context.User as SocketGuildUser;
            if (_songService.VoiceChannel != null)
            {
                if (user?.VoiceChannel == null || !_songService.VoiceChannel.Id.Equals(user.VoiceChannel.Id))
                {
                    await ReplyAsync("You must be in the same voice channel as the bot for this to work.");
                    return false;
                }
            }
            else if (user?.VoiceChannel == null)
            {
                await ReplyAsync("You must be in a voice channel to listen to music.");
                return false;
            }
            return true;
        }

        private async Task Play(string name)
        {
            try
            {
                //await Context.Message.DeleteAsync();
                if (!Uri.IsWellFormedUriString(name, UriKind.Absolute))
                {
                    await DownloadSearch(name);
                    return;
                }

                if (name.Contains("results?search_query"))
                {
                    await ReplyAsync("I cannot find videos based of a search link (yet).");
                    return;
                }
                if (name.Contains("playlist?list=")) await DownloadPlaylist(name);
                else await DownloadUrl(name);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while processing song request: {e}");
            }
        }

        private async Task DownloadPlaylist(string url)
        {
            List<YoutubeVideo> videos;
            var notice = await ReplyAsync("Downloading playlist... This may take a while if it's a large playlist.");
            try
            {
                videos = await YoutubeDownloadService.DownloadPlaylist(url);
                if (videos == null || !videos.Any() || videos.All(v => v == null)) throw new Exception();
            }
            catch (Exception e)
            {
                await ReplyAsync("I was unable to download your requested video. Please contact D-Kay#0666 if the issue persists.");
                Console.WriteLine($"Error while downloading playlist: {e}");
                return;
            }
            finally
            {
                await notice.DeleteAsync();
            }

            var invalidVideo = false;
            var count = 0;
            foreach (var video in videos)
            {
                if (video == null)
                {
                    Console.WriteLine("Video in playlist was null.");
                    invalidVideo = true;
                    continue;
                }
                QueueVideo(video);
                count++;
            }

            await ReplyAsync($"Added `{count}` songs to the queue.");
            if (invalidVideo) await ReplyAsync("I could not download some of the videos in the playlist.");
        }

        private async Task DownloadUrl(string url)
        {
            YoutubeVideo video;
            var notice = await ReplyAsync($"Downloading `{url}`.....");
            try
            {
                video = await YoutubeDownloadService.DownloadUrl(url);
                if (video == null) throw new Exception();
            }
            catch (Exception e)
            {
                await ReplyAsync("I was unable to find a downloadable video. Are you sure it's a valid video url? Please contact D-Kay#0666 if the issue persists.");
                Console.WriteLine($"Error while downloading url: {e}");
                return;
            }
            finally
            {
                await notice.DeleteAsync();
            }

            var position = QueueVideo(video);
            await ReplyAsync($"Queued **{video.Title}** (`{TimeSpan.FromSeconds(video.Duration)}`). Position in queue: #{position}");
        }

        private async Task DownloadSearch(string name)
        {
            YoutubeVideo video;
            var notice = await ReplyAsync($"Searching and downloading `{name}`...");
            try
            {
                video = await YoutubeDownloadService.DownloadSearch(name);
                if (video == null) throw new Exception();
            }
            catch (Exception e)
            {
                await ReplyAsync($"I could not find a video for you. Could you specify your desired video more clearly?");
                Console.WriteLine($"Error while downloading video: {e}");
                return;
            }
            finally
            {
                await notice.DeleteAsync();
            }

            var position = QueueVideo(video);
            await ReplyAsync($"Queued **{video.Title}** (`{TimeSpan.FromSeconds(video.Duration)}`). Position in queue: #{position}");
        }

        private int QueueVideo(YoutubeVideo video)
        {
            video.Requester = Context.User;
            video.TextChannel = Context.Channel;
            return _songService.Queue(video);
        }
    }
}
