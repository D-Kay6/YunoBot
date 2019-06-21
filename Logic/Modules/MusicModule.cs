using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Logic.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Victoria.Entities;

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

        private Localization.Localization _lang;

        protected override void BeforeExecute(CommandInfo command)
        {
            AudioService.BeforeExecute(Context.Guild.Id);
            _lang = new Localization.Localization(Context.Guild.Id);
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
            var message = await ReplyAsync(_lang.GetMessage("Music search", query));
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
                    await Context.Channel.SendMessageAsync(_lang.GetMessage("Music queued playlist", result.Tracks.Count()));
                    break;
                case LoadType.NoMatches:
                    await ReplyAsync(_lang.GetMessage("Music invalid song"));
                    break;
                case LoadType.LoadFailed:
                    await ReplyAsync(_lang.GetMessage("Music exception"));
                    break;
            }
        }

        [Command("shuffle")]
        public async Task MusicShuffle()
        {
            if (!AudioService.IsPlaying)
            {
                await ReplyAsync(_lang.GetMessage("Music not active"));
                return;
            }

            if (!await CanPerform()) return;
            
            var notice = await ReplyAsync(_lang.GetMessage("Music shuffle"));
            AudioService.Shuffle();
            await notice.DeleteAsync();
            await ReplyAsync(_lang.GetMessage("Music shuffled"));
        }

        [Command("skip")]
        public async Task MusicSkip()
        {
            if (!AudioService.IsPlaying)
            {
                await ReplyAsync(_lang.GetMessage("Music not active"));
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
                await ReplyAsync(_lang.GetMessage("Music not active"));
                return;
            }

            if (!await CanPerform()) return;
            await AudioService.TextChannel.SendMessageAsync(_lang.GetMessage("Music stop"));
            await AudioService.Stop();
        }

        [Command("clear")]
        public async Task MusicClear()
        {
            if (!AudioService.IsPlaying)
            {
                await ReplyAsync(_lang.GetMessage("Music not active"));
                return;
            }

            if (!await CanPerform()) return;
            AudioService.Clear();
            await ReplyAsync(_lang.GetMessage("Music clear"));
        }

        [Alias("now")]
        [Command("playing")]
        public async Task MusicPlaying()
        {
            var currentSong = AudioService.CurrentTrack;
            if (currentSong == null) await ReplyAsync(_lang.GetMessage("Music current empty"));
            else await ReplyAsync(_lang.GetMessage("Music current item", currentSong.Title));
        }
        
        [Command("queue")]
        public async Task MusicQueue()
        {
            if (!AudioService.IsPlaying)
            {
                await ReplyAsync(_lang.GetMessage("Music not active"));
                return;
            }

            var queue = AudioService.GetQueue();
            if (!queue.Any())
            {
                await ReplyAsync(_lang.GetMessage("Music queue empty"));
                return;
            }
            var position = 1;
            var msg = "";
            foreach (var item in queue)
            {
                msg += _lang.GetMessage("Music queue item", position, item.Track.Title, item.Track.Length);
                msg += "\n";
                if (position >= 15) break;
                position++;
            }
            await ReplyAsync(msg);
            if (queue.Count - 15 > 0) await ReplyAsync(_lang.GetMessage("Music queue remaining", queue.Count - 15));
        }

        [Command("pause")]
        public async Task MusicPause()
        {
            if (!AudioService.IsPlaying)
            {
                await ReplyAsync(_lang.GetMessage("Music not active"));
                return;
            }

            if (!await CanPerform()) return;

            await ReplyAsync(_lang.GetMessage(await AudioService.Pause() ? "Music paused" : "Music is paused"));
        }

        [Command("unpause")]
        public async Task MusicUnPause()
        {
            if (!AudioService.IsPlaying)
            {
                await ReplyAsync(_lang.GetMessage("Music not active"));
                return;
            }

            if (!await CanPerform()) return;

            await ReplyAsync(_lang.GetMessage(await AudioService.Play() ? "Music unpaused" : "Music is paused"));
        }

        [Command("volume")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task MusicVolume(int volume)
        {
            if (!AudioService.IsPlaying)
            {
                await ReplyAsync(_lang.GetMessage("Music not active"));
                return;
            }

            if (!await CanPerform()) return;
            if (volume > 100) volume = 100;
            if (volume < 0) volume = 0;
            await AudioService.ChangeVolume(volume);

            await ReplyAsync(_lang.GetMessage("Music volume", volume));
        }

        private async Task<bool> CanPerform()
        {
            var user = Context.User as SocketGuildUser;
            var voiceChannel = AudioService.VoiceChannel;
            if (voiceChannel != null)
            {
                if (user?.VoiceChannel != null && voiceChannel.Id.Equals(user.VoiceChannel.Id)) return true;
                await ReplyAsync(_lang.GetMessage("Music channel same"));
                return false;
            }

            if (user?.VoiceChannel != null) return true;
            await ReplyAsync(_lang.GetMessage("Music channel none"));
            return false;
        }
    }
}