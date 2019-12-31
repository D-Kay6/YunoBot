using System;
using Discord.WebSocket;
using IDal.Database;
using Logic.Models.Music;
using Logic.Services;
using System.Linq;
using System.Threading.Tasks;
using Victoria;
using Victoria.Enums;
using Victoria.EventArgs;

namespace Logic.Handlers
{
    public class MusicHandler : BaseHandler
    {
        private readonly IDbLanguage _language;
        private readonly LocalizationService _localization;
        private readonly MusicService _music;
        private readonly LogsService _logs;

        private readonly LavaNode _lavaNode;

        public MusicHandler(DiscordSocketClient client, IDbLanguage language, LocalizationService localization, MusicService music, LogsService logs) : base(client)
        {
            _language = language;
            _localization = localization;
            _music = music;
            _logs = logs;

            _lavaNode = music.Lavanode;
        }

        public override async Task Initialize()
        {
            Client.UserVoiceStateUpdated += HandleChannelAsync;
            Client.Ready += OnReady;
        }

        private async Task HandleChannelAsync(SocketUser user, SocketVoiceState leaveState, SocketVoiceState joinState)
        {
            try
            {
                var channel = leaveState.VoiceChannel;
                if (channel == null) return;
                if (channel.Users.Count != 1) return;
                if (!channel.Users.First().Id.Equals(Client.CurrentUser.Id)) return;
                await _music.BeforeExecute(channel.Guild);
                await _localization.Load(await _language.GetLanguage(channel.Guild.Id));
                await _music.TextChannel.SendMessageAsync(_localization.GetMessage("Music stop channel"));
                await _music.Stop();
            }
            catch (Exception e)
            {
                await _logs.Write("Crashes", $"HandleChannelAsync failed. Reason: {e.Message}, Stacktrace: {e.StackTrace}.");
            }
        }

        private async Task OnReady()
        {
            await _lavaNode.ConnectAsync();
            _lavaNode.OnTrackException += OnTrackException;
            _lavaNode.OnTrackStuck += OnTrackStuck;
            _lavaNode.OnTrackEnded += OnTrackEnded;
        }

        private async Task OnTrackException(TrackExceptionEventArgs e)
        {
            await _logs.Write("Crashes", $"Could not play track {e.Track.Title}. Reason: {e.ErrorMessage}.");
            await _music.BeforeExecute(e.Player.VoiceChannel.Guild);
            await _music.PlayNextTrack();
        }

        private async Task OnTrackStuck(TrackStuckEventArgs e)
        {
            await _music.BeforeExecute(e.Player.VoiceChannel.Guild);
            await _music.PlayNextTrack();
        }

        private async Task OnTrackEnded(TrackEndedEventArgs e)
        {
            switch (e.Reason)
            {
                case TrackEndReason.Finished:
                    await _music.BeforeExecute(e.Player.VoiceChannel.Guild);
                    await _music.PlayNextTrack();
                    break;
                case TrackEndReason.Replaced:
                    break;
                case TrackEndReason.LoadFailed:
                    await _music.BeforeExecute(e.Player.VoiceChannel.Guild);
                    await _music.PlayNextTrack();
                    break;
                case TrackEndReason.Cleanup:
                    break;
                case TrackEndReason.Stopped:
                    await _music.BeforeExecute(e.Player.VoiceChannel.Guild);
                    await _music.EndPlayer();
                    break;
            }
        }
    }
}