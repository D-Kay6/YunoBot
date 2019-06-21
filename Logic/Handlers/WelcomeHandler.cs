using DalFactory;
using Discord.WebSocket;
using IDal.Interfaces.Database;
using Logic.Extentions;
using System.Threading.Tasks;

namespace Logic.Handlers
{
    public class WelcomeHandler
    {
        private DiscordSocketClient _client;
        private IWelcomeMessage _welcome;

        public async Task Initialize(DiscordSocketClient client)
        {
            _client = client;
            _welcome = DatabaseFactory.GenerateWelcomeMessage();
            _client.UserJoined += OnUserJoined;
        }

        private async Task OnUserJoined(SocketGuildUser user)
        {
            var welcome = _welcome.GetWelcomeMessage(user.Guild.Id);
            if (welcome.ChannelId == null) return;
            if (!(user.Guild.GetChannel((ulong) welcome.ChannelId) is ISocketMessageChannel channel))
            {
                await DisableWelcomeMessage(user.Guild);
                return;
            }

            var msg = string.Format(welcome.Message, user.Mention);
            if (!welcome.UseImage) await channel.SendMessageAsync(msg);
            else await channel.SendFileAsync(ImageExtentions.GetImagePath("GasaiYunoWelcome.jpg"), msg);
        }

        private async Task DisableWelcomeMessage(SocketGuild guild)
        {
            _welcome.Disable(guild.Id);
            var settings = DatabaseFactory.GenerateServerSettings();
            var owner = guild.Owner;
            await owner.SendDM($@"I'm sorry to bother you, but it seems like something went wrong with the welcome message for `{guild.Name}`.
This could have happened due to an accidental deletion and re-creation of the channel.
As a result of this, I had to disable the feature.
You can re-enable it by executing the command `{settings.GetCommandPrefix(guild.Id)}welcome enable <channel>` on your server again.

Sorry for the inconvenience.
If this keeps happening, join my support server 'https://discord.gg/YFqUMDT' to get some help.");
        }
    }
}
