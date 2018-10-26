using System;
using System.Collections.Generic;
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
    public class MusicModule : ModuleBase<SocketCommandContext>
    {
        public YouTubeDownloadService YoutubeDownloadService { get; set; }

        public SongService SongService { get; set; }

        [Alias("song")]
        [Summary("Requests a song to be played")]
        [Command("music")]
        public async Task Command([Remainder]string arguments = null)
        {
            if (string.IsNullOrWhiteSpace(arguments))
            {
                await SendHelpInfo();
                return;
            }
            var args = arguments.Split(' ');
            switch (args.First().ToLower())
            {
                case "help":
                    await SendHelpInfo();
                    break;
                case "test":
                    await SoundTest();
                    break;
                case "request":
                    await Request(args.Skip(1));
                    break;
                case "skip":
                    await SkipSong();
                    break;
                case "clear":
                    await ClearQueue();
                    break;
                case "playing":
                    await NowPlaying();
                    break;
                default:
                    await ReplyAsync("Incorrect command usage.");
                    return;
            }
        }

        private async Task SoundTest()
        {
            await Request(new[] { "https://www.youtube.com/watch?v=i1GOn7EIbLg" });
        }

        private async Task Request(IEnumerable<string> args)
        {
            var user = Context.User as SocketGuildUser;
            if (user.VoiceChannel == null)
            {
                await ReplyAsync("You must be in a voice channel.");
                return;
            }
            if (SongService.NowPlaying == null)
            {
                SongService.SetVoiceChannel(user.VoiceChannel);
                SongService.SetMessageChannel(Context.Channel);
            }
            else if (!SongService.VoiceChannel.Id.Equals(user.VoiceChannel.Id))
            {
                await ReplyAsync("You must be in the same voice channel as the bot.");
                return;
            }
            await Play(string.Join(" ", args), 48);
        }

        private async Task Play(string url, int speedModifier)
        {
            try
            {
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    await ReplyAsync($"{Context.User.Mention} please provide a valid song URL");
                    return;
                }

                var downloadAnnouncement = await ReplyAsync($"{Context.User.Mention} attempting to download {url}");
                var video = await YoutubeDownloadService.DownloadVideo(url);
                await downloadAnnouncement.DeleteAsync();

                if (video == null)
                {
                    await ReplyAsync($"{Context.User.Mention} unable to queue song, make sure its is a valid supported URL or contact a server admin.");
                    return;
                }

                video.Requester = Context.User.Mention;
                video.Speed = speedModifier;

                await ReplyAsync($"{Context.User.Mention} queued **{video.Title}** | `{TimeSpan.FromSeconds(video.Duration)}` | {url}");

                SongService.Queue(video);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error while processing song requet: {e}");
            }
        }

        private async Task SkipSong()
        {
            SongService.Next();
            await ReplyAsync("Skipped song");
        }

        private async Task ClearQueue()
        {
            SongService.Clear();
            await ReplyAsync("Queue cleared");
        }

        private async Task NowPlaying()
        {
            if (SongService.NowPlaying == null)
            {
                await ReplyAsync($"{Context.User.Mention} current queue is empty");
            }
            else
            {
                await ReplyAsync($"{Context.User.Mention} now playing `{SongService.NowPlaying.Title}` requested by {SongService.NowPlaying.Requester}");
            }
        }

        private async Task SendHelpInfo()
        {
            var msg = $@" /music help - show the music command information.

Hope this helps you :grin:.";
            var embed = EmbedExtention.CreateEmbed("Music commands", msg, Color.Red);
            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
