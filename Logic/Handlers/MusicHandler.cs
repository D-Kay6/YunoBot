using Discord;
using Discord.WebSocket;
using IDal.Database;
using Logic.Exceptions;
using Logic.Extensions;
using Logic.Models.Music;
using Logic.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Handlers
{
    public class MusicHandler : BaseHandler
    {
        private readonly MusicService _music;
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;
        private readonly LogsService _logs;

        public MusicHandler(DiscordSocketClient client, MusicService music, IDbLanguage language, LocalizationService localization, LogsService logs) : base(client)
        {
            _music = music;
            _language = language;
            _localization = localization;
            _logs = logs;
        }

        public override Task Initialize()
        {
            Client.Ready += OnReady;
            Client.UserVoiceStateUpdated += OnChangeVoiceChannel;

            _music.Player.TrackException += OnTrackException;
            _music.Player.TrackStuck += OnTrackStuck;
            _music.Player.TrackEnded += OnTrackEnded;
            return Task.CompletedTask;
        }

        private async Task OnReady()
        {
            await _music.Player.Ready();
        }

        private async Task Prepare(IGuild guild)
        {
            await _localization.Load(await _language.GetLanguage(guild.Id));
            await _music.Prepare(guild);
        }

        private async Task OnChangeVoiceChannel(SocketUser user, SocketVoiceState leaveState, SocketVoiceState joinState)
        {
            var voiceChannel = leaveState.VoiceChannel;
            var textChannel = _music.Player.TextChannel;
            if (voiceChannel == null) return;
            try
            {
                if (voiceChannel.Users.Count != 1) return;
                if (!voiceChannel.Users.First().Id.Equals(Client.CurrentUser.Id)) return;

                await Prepare(voiceChannel.Guild);
                try
                {
                    await _music.Stop();
                    await textChannel.SendMessageAsync(_localization.GetMessage("Music stop channel"));
                }
                catch (Exception e) when (e is InvalidPlayerException || e is InvalidTrackException)
                {
                    await _music.Leave(voiceChannel);
                }
            }
            catch (Exception e)
            {
                await _logs.Write("Crashes", $"Failed to handle voice channel change for music handler. Reason: {e.Message}, StackTrace: {e.StackTrace}");
            }
        }

        private async Task OnTrackException(TrackExceptionEventArgs e)
        {
            await _logs.Write("Crashes", $"Could not play track {e.Track.Title}. Reason: {e.Message}");
            await Prepare(e.Player.VoiceChannel.Guild);
            var track = await _music.PlayNext();
            await track.TextChannel.SendMessageAsync(_localization.GetMessage("Music now playing", track.Track.Title, track.Requester.Nickname()));
        }

        private async Task OnTrackStuck(TrackStuckEventArgs e)
        {
            await _logs.Write("Crashes", $"Track {e.Track.Title} got stuck. Time: {e.Duration}");
            await Prepare(e.Player.VoiceChannel.Guild);
            var track = await _music.PlayNext();
            await track.TextChannel.SendMessageAsync(_localization.GetMessage("Music now playing", track.Track.Title, track.Requester.Nickname()));
        }

        private async Task OnTrackEnded(TrackEndedEventArgs e)
        {
            IPlayable track = null;
            switch (e.Reason)
            {
                case "Finished":
                    await Prepare(e.Player.VoiceChannel.Guild);
                    track = await _music.PlayNext();
                    break;
                case "Replaced":
                    break;
                case "LoadFailed":
                    await Prepare(e.Player.VoiceChannel.Guild);
                    await e.Player.TextChannel.SendMessageAsync(_localization.GetMessage("Music exception"));
                    track = await _music.PlayNext();
                    break;
                case "Cleanup":
                    break;
                case "Stopped":
                    break;
            }

            if (track == null) return;

            await track.TextChannel.SendMessageAsync(_localization.GetMessage("Music now playing", track.Track.Title, track.Requester.Nickname()));
        }
    }
}