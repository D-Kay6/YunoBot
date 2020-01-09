using Discord;
using Discord.Commands;
using Discord.WebSocket;
using IDal.Database;
using Logic.Models.Music.Track;
using Logic.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Victoria.Enums;

namespace Logic.Modules
{
    [Group("music")]
    public class MusicModule : ModuleBase<SocketCommandContext>
    {
        private readonly MusicService _musicService;

        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;

        public MusicModule(MusicService musicService, IDbLanguage language, LocalizationService localization)
        {
            _musicService = musicService;
            _language = language;
            _localization = localization;
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            Task.WaitAll(Prepare(), _musicService.Prepare(Context.Guild));
            base.BeforeExecute(command);
        }

        private async Task Prepare()
        {
            await _localization.Load(await _language.GetLanguage(Context.Guild.Id));
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
            var message = await ReplyAsync(_localization.GetMessage("Music search", query));
            var result = await _musicService.GetTracks(query);
            await message.DeleteAsync();
            switch (result.LoadStatus)
            {
                case LoadStatus.SearchResult:
                    var search = result.Tracks.FirstOrDefault(t => query.Contains(t.Id)) ?? result.Tracks.First();
                    await _musicService.Queue(new Song(search, Context));
                    break;
                case LoadStatus.TrackLoaded:
                    var track = result.Tracks.First();
                    await _musicService.Queue(new Song(track, Context));
                    break;
                case LoadStatus.PlaylistLoaded:
                    var user = (SocketGuildUser) Context.User;
                    await _musicService.Queue(result.Tracks.Select(t => new Song(t, Context)), user.VoiceChannel);
                    await Context.Channel.SendMessageAsync(_localization.GetMessage("Music queued playlist", result.Tracks.Count));
                    break;
                case LoadStatus.NoMatches:
                    await ReplyAsync(_localization.GetMessage("Music invalid song"));
                    break;
                case LoadStatus.LoadFailed:
                    await ReplyAsync(_localization.GetMessage("Music exception"));
                    break;
            }
        }

        [Command("shuffle")]
        public async Task MusicShuffle()
        {
            if (!_musicService.IsPlaying)
            {
                await ReplyAsync(_localization.GetMessage("Music not active"));
                return;
            }

            if (!await CanPerform()) return;
            
            var notice = await ReplyAsync(_localization.GetMessage("Music shuffle"));
            _musicService.Shuffle();
            await notice.DeleteAsync();
            await ReplyAsync(_localization.GetMessage("Music shuffled"));
        }

        [Command("skip")]
        public async Task MusicSkip()
        {
            if (!_musicService.IsPlaying)
            {
                await ReplyAsync(_localization.GetMessage("Music not active"));
                return;
            }

            if (!await CanPerform()) return;

            await _musicService.Skip();
        }

        [Command("skip")]
        public async Task MusicSkip(int amount)
        {
            if (!_musicService.IsPlaying)
            {
                await ReplyAsync(_localization.GetMessage("Music not active"));
                return;
            }

            if (!await CanPerform()) return;

            await _musicService.Skip(amount);
            await ReplyAsync(_localization.GetMessage("Music skipped", amount));
        }

        [Command("stop")]
        public async Task MusicStop()
        {
            if (!_musicService.IsPlaying)
            {
                await ReplyAsync(_localization.GetMessage("Music not active"));
                return;
            }

            if (!await CanPerform()) return;
            await _musicService.TextChannel.SendMessageAsync(_localization.GetMessage("Music stop"));
            await _musicService.Stop();
        }

        [Command("clear")]
        public async Task MusicClear()
        {
            if (!_musicService.IsPlaying)
            {
                await ReplyAsync(_localization.GetMessage("Music not active"));
                return;
            }

            if (!await CanPerform()) return;
            _musicService.Clear();
            await ReplyAsync(_localization.GetMessage("Music clear"));
        }

        [Alias("now")]
        [Command("playing")]
        public async Task MusicPlaying()
        {
            var currentSong = _musicService.CurrentTrack;
            if (currentSong == null) await ReplyAsync(_localization.GetMessage("Music current empty"));
            else await ReplyAsync(_localization.GetMessage("Music current item", currentSong.Title));
        }
        
        [Command("queue")]
        public async Task MusicQueue()
        {
            if (!_musicService.IsPlaying)
            {
                await ReplyAsync(_localization.GetMessage("Music not active"));
                return;
            }

            var queue = _musicService.GetQueue();
            if (!queue.Any())
            {
                await ReplyAsync(_localization.GetMessage("Music queue empty"));
                return;
            }
            var position = 1;
            var msg = "";
            foreach (var item in queue)
            {
                msg += _localization.GetMessage("Music queue item", position, item.Track.Title, item.Track.Duration);
                msg += "\n";
                if (position >= 15) break;
                position++;
            }
            await ReplyAsync(msg);
            if (queue.Count - 15 > 0) await ReplyAsync(_localization.GetMessage("Music queue remaining", queue.Count - 15));
        }

        [Command("pause")]
        public async Task MusicPause()
        {
            if (!_musicService.IsPlaying)
            {
                await ReplyAsync(_localization.GetMessage("Music not active"));
                return;
            }

            if (!await CanPerform()) return;

            await ReplyAsync(_localization.GetMessage(await _musicService.Pause() ? "Music paused" : "Music is paused"));
        }

        [Command("unpause")]
        public async Task MusicUnPause()
        {
            if (!_musicService.IsPlaying)
            {
                await ReplyAsync(_localization.GetMessage("Music not active"));
                return;
            }

            if (!await CanPerform()) return;

            await ReplyAsync(_localization.GetMessage(await _musicService.Play() ? "Music unpaused" : "Music is paused"));
        }

        [Command("volume")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task MusicVolume(ushort volume)
        {
            if (!_musicService.IsPlaying)
            {
                await ReplyAsync(_localization.GetMessage("Music not active"));
                return;
            }

            if (!await CanPerform()) return;
            if (volume > 150) volume = 150;
            await _musicService.ChangeVolume(volume);

            await ReplyAsync(_localization.GetMessage("Music volume", volume));
        }

        private async Task<bool> CanPerform()
        {
            var user = Context.User as SocketGuildUser;
            var voiceChannel = _musicService.VoiceChannel;
            if (voiceChannel != null)
            {
                if (user?.VoiceChannel != null && voiceChannel.Id.Equals(user.VoiceChannel.Id)) return true;
                await ReplyAsync(_localization.GetMessage("Music channel same"));
                return false;
            }

            if (user?.VoiceChannel != null) return true;
            await ReplyAsync(_localization.GetMessage("Music channel none"));
            return false;
        }
    }
}