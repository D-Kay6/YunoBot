using System;
using System.Linq;
using Discord.Commands;
using Discord.WebSocket;
using Logic.Services;
using System.Threading.Tasks;
using Discord;
using Logic.Extentions;
using Victoria.Entities;
using SearchResult = Victoria.Entities.SearchResult;

namespace Logic.Modules
{
    [Group("music")]
    public class MusicModule : ModuleBase<SocketCommandContext>
    {
        public MusicModule(AudioService audioService)
        {
            AudioService = audioService;
        }

        private AudioService AudioService { get; }

        protected override void BeforeExecute(CommandInfo command)
        {
            AudioService.BeforeExecute(Context.Guild.Id);
            base.BeforeExecute(command);
        }

        [Priority(-1)]
        [Command]
        public async Task DefaultMusic()
        {
            await MusicPlaying();
        }

        [Alias("request")]
        [Command("play")]
        [Summary("Requests a song to be played")]
        public async Task MusicPlay([Remainder] string query)
        {
            if (!await CanPerform()) return;
            if (query.Contains("&list=")) query = query.Substring(0, query.IndexOf("&list=", StringComparison.CurrentCulture));
            var message = await ReplyAsync($"Searching for `{query}`...");
            var result = await AudioService.GetTracks(query);
            await message.DeleteAsync();
            switch (result.LoadType)
            {
                case LoadType.SearchResult:
                    var search = result.Tracks.FirstOrDefault(t => query.Contains(t.Id)) ?? result.Tracks.First();
                    await AudioService.Queue(new Song(search, Context));
                    break;
                case LoadType.TrackLoaded:
                    var track = result.Tracks.First();
                    await AudioService.Queue(new Song(track, Context));
                    break;
                case LoadType.PlaylistLoaded:
                    var user = (SocketGuildUser) Context.User;
                    await AudioService.Queue(result.Tracks.Select(t => new Song(t, Context)), user.VoiceChannel);
                    await Context.Channel.SendMessageAsync($"Queued {result.Tracks.Count()} songs.");
                    break;
                case LoadType.LoadFailed:
                    await ReplyAsync("Something went wrong. Come to the support server if this problem persists.");
                    break;
                case LoadType.NoMatches:
                    await ReplyAsync("I could not find a valid song to play.");
                    break;
            }
        }

        [Command("shuffle")]
        public async Task MusicShuffle()
        {
            if (!AudioService.IsPlaying)
            {
                await ReplyAsync("I'm currently not playing music.");
                return;
            }

            if (!await CanPerform()) return;
            
            var notice = await ReplyAsync("Shuffling...");
            AudioService.Shuffle();
            await notice.DeleteAsync();
            await ReplyAsync("The queue has been shuffled.");
        }

        [Command("skip")]
        public async Task MusicSkip()
        {
            if (!AudioService.IsPlaying)
            {
                await ReplyAsync("I'm currently not playing music.");
                return;
            }

            if (!await CanPerform()) return;

            await AudioService.Skip();
        }

        [Command("stop")]
        public async Task MusicStop()
        {
            if (!AudioService.IsPlaying)
            {
                await ReplyAsync("I'm currently not playing music.");
                return;
            }

            if (!await CanPerform()) return;
            await AudioService.TextChannel.SendMessageAsync("The music player was stopped.");
            await AudioService.Stop();
        }

        [Command("clear")]
        public async Task MusicClear()
        {
            if (!AudioService.IsPlaying)
            {
                await ReplyAsync("I'm currently not playing music.");
                return;
            }

            if (!await CanPerform()) return;
            AudioService.Clear();
            await ReplyAsync("Queue cleared :ok_hand:.");
        }

        [Alias("now")]
        [Command("playing")]
        public async Task MusicPlaying()
        {
            var currentSong = AudioService.CurrentTrack;
            if (currentSong == null)
                await ReplyAsync($"{Context.User.Mention} current queue is empty");
            else
                await ReplyAsync($"{Context.User.Mention} now playing `{currentSong.Title}`.");
        }
        
        [Command("queue")]
        public async Task MusicQueue()
        {
            if (!AudioService.IsPlaying)
            {
                await ReplyAsync("I'm currently not playing music.");
                return;
            }

            var queue = AudioService.GetQueue();
            if (!queue.Any())
            {
                await ReplyAsync("There is no music in the queue. Use /music play to add some music.");
                return;
            }
            var position = 1;
            var msg = "";
            foreach (var item in queue)
            {
                msg += $"#{position}  **{item.Track.Title}** (`{item.Track.Length}`).\n";
                if (position >= 15) break;
                position++;
            }
            await ReplyAsync(msg);
            await ReplyAsync($"\nThere are {queue.Count - 15} more songs in queue.");
        }

        [Command("pause")]
        public async Task MusicPause()
        {
            if (!AudioService.IsPlaying)
            {
                await ReplyAsync("I'm currently not playing music.");
                return;
            }

            if (!await CanPerform()) return;

            if (await AudioService.Pause()) await ReplyAsync("Music player is paused.");
            else await ReplyAsync("The music player is already paused.");
        }

        [Command("unpause")]
        public async Task MusicUnPause()
        {
            if (!AudioService.IsPlaying)
            {
                await ReplyAsync("I'm currently not playing music.");
                return;
            }

            if (!await CanPerform()) return;

            if (await AudioService.Play()) await ReplyAsync("Music player is un-paused.");
            else await ReplyAsync("The music player is not paused.");
        }

        [Command("volume")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task MusicVolume(int volume)
        {
            if (!AudioService.IsPlaying)
            {
                await ReplyAsync("I'm currently not playing music.");
                return;
            }

            if (!await CanPerform()) return;
            if (volume > 100) volume = 100;
            if (volume < 0) volume = 0;
            await AudioService.ChangeVolume(volume);

            await ReplyAsync($"Music volume was changed to {volume}.");
        }

        private async Task<bool> CanPerform()
        {
            var user = Context.User as SocketGuildUser;
            var voiceChannel = AudioService.VoiceChannel;
            if (voiceChannel != null)
            {
                if (user?.VoiceChannel != null && voiceChannel.Id.Equals(user.VoiceChannel.Id)) return true;
                await ReplyAsync("You must be in the same voice channel as me to be able to do this.");
                return false;
            }

            if (user?.VoiceChannel != null) return true;
            await ReplyAsync("You must be in a voice channel to listen to music.");
            return false;
        }
    }
}