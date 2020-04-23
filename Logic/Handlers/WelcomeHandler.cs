using Logic.Exceptions;

namespace Logic.Handlers
{
    using System.Threading.Tasks;
    using Discord.WebSocket;
    using Extensions;
    using Services;

    public class WelcomeHandler : BaseHandler
    {
        private readonly CommandService _command;
        private readonly WelcomeService _welcome;

        public WelcomeHandler(DiscordSocketClient client, CommandService command, WelcomeService welcome) : base(client)
        {
            _command = command;
            _welcome = welcome;
        }

        public override Task Initialize()
        {
            Client.UserJoined += OnUserJoined;
            return Task.CompletedTask;
        }

        private async Task OnUserJoined(SocketGuildUser user)
        {
            try
            {
                await _welcome.Welcome(user);
            }
            catch (InvalidChannelException)
            {
                await DisableWelcomeMessage(user.Guild);
            }
            catch (InvalidWelcomeException)
            {
                // Ignore
            }
        }

        private async Task DisableWelcomeMessage(SocketGuild guild)
        {
            var settings = await _welcome.Load(guild.Id);
            settings.ChannelId = null;
            await _welcome.Save(settings);

            var owner = guild.Owner;
            await owner.SendDM(
                $@"I'm sorry to bother you, but it seems like something went wrong with the welcome message for `{guild.Name}`.
This could have happened due to an accidental deletion and re-creation of the channel.
As a result of this, I had to disable the feature.
You can re-enable it by executing the command `{_command.GetPrefix(guild.Id)}welcome enable <channel>` on your server again.

Sorry for the inconvenience.
If this keeps happening, join my support server 'https://discord.gg/YFqUMDT' to get some help.");
        }
    }
}