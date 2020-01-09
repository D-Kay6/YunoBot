using Discord.WebSocket;
using Logic.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using Logic.Models.Music;

namespace Logic.Handlers
{
    public class MusicHandler : BaseHandler
    {
        private readonly MusicService _music;
        private readonly LogsService _logs;

        public MusicHandler(DiscordSocketClient client, MusicService music, LogsService logs) : base(client)
        {
            _music = music;
            _logs = logs;
        }

        public override async Task Initialize()
        {
            Client.Ready += OnReady;
            Client.UserVoiceStateUpdated += OnChangeVoiceChannel;

            _lavaNode.TrackException += OnTrackException;
            _lavaNode.TrackStuck += OnTrackStuck;
            _lavaNode.TrackEnded += OnTrackEnded;
        }

        private async Task OnReady()
        {

        }

        private async Task OnChangeVoiceChannel(SocketUser user, SocketVoiceState leaveState, SocketVoiceState joinState)
        {
            var channel = leaveState.VoiceChannel;
            if (channel == null) return;
            try
            {
                if (channel.Users.Count != 1) return;
                if (!channel.Users.First().Id.Equals(Client.CurrentUser.Id)) return;

                await _music.Prepare(channel.Guild);
                await _music.Stop("channel");
            }
            catch (Exception e)
            {
                await _logs.Write("Crashes", $"Failed to handle voice channel change for music handler. Reason: {e.Message}, StackTrace: {e.StackTrace}");
            }
        }

        private async Task OnTrackException(TrackExceptionEventArgs e)
        {
            await _logs.Write("Crashes", $"Could not play track {e.Track.Title}. Reason: {e.Message}");
            await _music.Prepare(e.Player.VoiceChannel.Guild);
            await _music.PlayNextTrack();
        }

        private async Task OnTrackStuck(TrackStuckEventArgs e)
        {
            await _logs.Write("Crashes", $"Track {e.Track.Title} got stuck. Time: {e.Duration}");
            await _music.Prepare(e.Player.VoiceChannel.Guild);
            await _music.PlayNextTrack();
        }

        private async Task OnTrackEnded(TrackEndedEventArgs e)
        {
            await _music.Prepare(e.Player.VoiceChannel.Guild);
            switch (e.Reason)
            {
                case "Finished":
                    await _music.PlayNextTrack();
                    break;
                case "Replaced":
                    break;
                case "LoadFailed":
                    await _music.PlayNextTrack();
                    break;
                case "Cleanup":
                    break;
                case "Stopped":
                    await _music.EndPlayer();
                    break;
            }
        }
    }
}