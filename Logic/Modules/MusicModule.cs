namespace Logic.Modules
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Discord;
    using Discord.Commands;
    using Discord.WebSocket;
    using Exceptions;
    using Extensions;
    using IDal.Database;
    using Models.Music;
    using Models.Music.Search;
    using Models.Music.Track;
    using Services;

    [Group("music")]
    public class MusicModule : ModuleBase<SocketCommandContext>
    {
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;
        private readonly MusicService _musicService;

        public MusicModule(MusicService musicService, IDbLanguage language, LocalizationService localization)
        {
            _musicService = musicService;
            _language = language;
            _localization = localization;
        }

        protected override void BeforeExecute(CommandInfo command)
        {
            Task.WaitAll(Prepare());
            base.BeforeExecute(command);
        }

        private async Task Prepare()
        {
            await _musicService.Prepare(Context.Guild);
            await _localization.Load(await _language.GetLanguage(Context.Guild.Id));
        }

        [Priority(-1)]
        [Command]
        public async Task DefaultMusic()
        {
            await MusicPlaying();
        }


        [Command("join")]
        public async Task MusicJoin()
        {
            var user = Context.User as SocketGuildUser;
            try
            {
                await _musicService.Join(user.VoiceChannel);
            }
            catch (InvalidPlayerException)
            {
                await ReplyAsync(_localization.GetMessage("Music connection exists"));
            }
        }

        [Command("move")]
        public async Task MusicMove()
        {
            var user = Context.User as SocketGuildUser;
            try
            {
                await _musicService.Move(user.VoiceChannel);
            }
            catch (InvalidPlayerException)
            {
                await ReplyAsync(_localization.GetMessage("Music connection none"));
            }
            catch (InvalidAudioChannelException)
            {
                await ReplyAsync(_localization.GetMessage("Music connection same"));
            }
        }

        [Command("leave")]
        public async Task MusicLeave()
        {
            var user = Context.User as SocketGuildUser;
            try
            {
                await _musicService.Leave(user.VoiceChannel);
            }
            catch (InvalidPlayerException)
            {
                await ReplyAsync(_localization.GetMessage("Music connection none"));
            }
            catch (InvalidAudioChannelException)
            {
                await ReplyAsync(_localization.GetMessage("Music connection different"));
            }
        }


        [Command("play")]
        [Alias("request")]
        [Summary("Requests a song to be played")]
        public async Task MusicPlay([Remainder] string query)
        {
            if (!await CanPerform()) return;

            var message = await ReplyAsync(_localization.GetMessage("Music search", query));
            if (query.Contains("&list="))
                query = query.Substring(0, query.IndexOf("&list=", StringComparison.CurrentCulture));
            var result = await _musicService.Search(query);
            await message.DeleteAsync();

            var user = (SocketGuildUser) Context.User;
            var index = 0;
            ITrack track = null;
            var isPlaying = _musicService.GetCurrentTrack() != null;
            switch (result.ResultStatus)
            {
                case ResultStatus.Failed:
                    await ReplyAsync(_localization.GetMessage("Music exception"));
                    break;
                case ResultStatus.NoMatch:
                    await ReplyAsync(_localization.GetMessage("Music invalid song"));
                    break;
                case ResultStatus.SearchResult:
                    track = result.Tracks.FirstOrDefault(t => query.Contains(t.Id)) ?? result.Tracks.First();
                    index = _musicService.Queue(new Playable(track, user, Context.Channel as ITextChannel));
                    if (isPlaying)
                    {
                        await ReplyAsync(_localization.GetMessage("Music queued song", index, track.Title,
                            track.Duration));
                        return;
                    }

                    break;
                case ResultStatus.SingleTrack:
                    track = result.Tracks.First();
                    index = _musicService.Queue(new Playable(track, user, Context.Channel as ITextChannel));
                    if (isPlaying)
                    {
                        await ReplyAsync(_localization.GetMessage("Music queued song", index, track.Title,
                            track.Duration));
                        return;
                    }

                    break;
                case ResultStatus.Playlist:
                    index = _musicService.Queue(result.Tracks.Select(t =>
                        new Playable(t, user, Context.Channel as ITextChannel)));
                    await Context.Channel.SendMessageAsync(_localization.GetMessage("Music queued playlist",
                        result.Tracks.Count));
                    break;
            }

            if (isPlaying) return;
            var trackInfo = await _musicService.PlayNext();
            await ReplyAsync(_localization.GetMessage("Music now playing", trackInfo.Track.Title,
                trackInfo.Requester.Nickname()));
        }

        [Command("pause")]
        public async Task MusicPause()
        {
            if (!await CanPerform()) return;

            try
            {
                await _musicService.Pause();
                await ReplyAsync(_localization.GetMessage("Music paused"));
            }
            catch (Exception e) when (e is InvalidPlayerException || e is InvalidTrackException)
            {
                await ReplyAsync(_localization.GetMessage("Music not active"));
            }
            catch (InvalidOperationException)
            {
                await ReplyAsync("Music already paused");
            }
        }

        [Command("resume")]
        public async Task MusicResume()
        {
            if (!await CanPerform()) return;

            try
            {
                await _musicService.Resume();
                await ReplyAsync(_localization.GetMessage("Music resumed"));
            }
            catch (Exception e) when (e is InvalidPlayerException || e is InvalidTrackException)
            {
                await ReplyAsync(_localization.GetMessage("Music not active"));
            }
            catch (InvalidOperationException)
            {
                await ReplyAsync("Music not paused");
            }
        }

        [Command("stop")]
        public async Task MusicStop()
        {
            if (!await CanPerform()) return;
            try
            {
                await _musicService.Stop();
                await ReplyAsync(_localization.GetMessage("Music stop command"));
            }
            catch (Exception e) when (e is InvalidPlayerException || e is InvalidTrackException)
            {
                await ReplyAsync(_localization.GetMessage("Music not active"));
            }
        }


        [Alias("now")]
        [Command("playing")]
        public async Task MusicPlaying()
        {
            var currentTrack = _musicService.GetCurrentTrack();
            if (currentTrack == null) await ReplyAsync(_localization.GetMessage("Music not active"));
            else await ReplyAsync(_localization.GetMessage("Music current", currentTrack.Title));
        }

        [Command("queue")]
        public async Task MusicQueue()
        {
            if (!_musicService.IsActive)
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
            if (queue.Count - 15 > 0)
                await ReplyAsync(_localization.GetMessage("Music queue remaining", queue.Count - 15));
        }


        [Command("shuffle")]
        public async Task MusicShuffle()
        {
            if (!await CanPerform()) return;
            if (!_musicService.HasQueue)
            {
                await ReplyAsync(_localization.GetMessage("Music queue empty"));
                return;
            }

            var notice = await ReplyAsync(_localization.GetMessage("Music shuffling"));
            _musicService.Shuffle();
            await notice.DeleteAsync();
            await ReplyAsync(_localization.GetMessage("Music shuffled"));
        }

        [Command("skip")]
        public async Task MusicSkip()
        {
            if (!await CanPerform()) return;

            try
            {
                var trackInfo = await _musicService.Skip();
                if (trackInfo == null) return;
                await ReplyAsync(_localization.GetMessage("Music now playing", trackInfo.Track.Title,
                    trackInfo.Requester.Nickname()));
            }
            catch (InvalidTrackException)
            {
                await ReplyAsync(_localization.GetMessage("Music not active"));
            }
        }

        [Command("skip")]
        public async Task MusicSkip(int amount)
        {
            if (!await CanPerform()) return;

            try
            {
                var trackInfo = await _musicService.Skip(amount);
                if (trackInfo == null) return;
                await ReplyAsync(_localization.GetMessage("Music skipped", amount));
                await ReplyAsync(_localization.GetMessage("Music now playing", trackInfo.Track.Title,
                    trackInfo.Requester.Nickname()));
            }
            catch (Exception e) when (e is InvalidPlayerException || e is InvalidTrackException)
            {
                await ReplyAsync(_localization.GetMessage("Music not active"));
            }
        }

        [Command("clear")]
        public async Task MusicClear()
        {
            if (!await CanPerform()) return;

            if (!_musicService.HasQueue)
            {
                await ReplyAsync(_localization.GetMessage("Music queue empty"));
                return;
            }

            _musicService.Clear();
            await ReplyAsync(_localization.GetMessage("Music queue cleared"));
        }


        [Command("volume")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task MusicVolume(ushort volume)
        {
            if (!await CanPerform()) return;

            try
            {
                if (volume > 150) volume = 150;
                await _musicService.ChangeVolume(volume);
                await ReplyAsync(_localization.GetMessage("Music volume", volume));
            }
            catch (InvalidPlayerException)
            {
                await ReplyAsync(_localization.GetMessage("Music not active"));
            }
        }


        private async Task<bool> CanPerform()
        {
            var user = Context.User as SocketGuildUser;
            var voiceChannel = _musicService.Player.VoiceChannel;
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