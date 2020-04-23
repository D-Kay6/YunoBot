using Core.Entity;
using Discord;
using IDal.Database;
using Logic.Exceptions;
using Logic.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class WelcomeService
    {
        private readonly IDbWelcome _dbWelcome;

        public WelcomeService(IDbWelcome dbWelcome)
        {
            _dbWelcome = dbWelcome;
        }

        /// <summary>
        ///     Get the settings for welcome messages.
        ///     Will reset corrupted values to default.
        /// </summary>
        /// <param name="serverId">The id of the server.</param>
        /// <returns>The welcome message settings.</returns>
        public async Task<WelcomeMessage> Load(ulong serverId)
        {
            var settings = await _dbWelcome.Get(serverId);

            if (settings == null)
            {
                settings = new WelcomeMessage
                {
                    ServerId = serverId
                };
                await _dbWelcome.Add(settings);
            }

            if (string.IsNullOrWhiteSpace(settings.Message))
            {
                settings.Message = "Welcome to the party {0}. Hope you will have a good time with us.";
                await _dbWelcome.Update(settings);
            }

            return settings;
        }

        /// <summary>
        ///     Save the new welcome message settings.
        /// </summary>
        /// <param name="settings">The new welcome message settings.</param>
        /// <exception cref="InvalidMessageException">Thrown if the message is null or empty.</exception>
        public async Task Save(WelcomeMessage settings)
        {
            if (string.IsNullOrWhiteSpace(settings.Message))
                throw new InvalidMessageException("The message cannot be null or empty.");

            await _dbWelcome.Update(settings);
        }

        /// <summary>
        ///     Send a welcome message to a user.
        /// </summary>
        /// <param name="user">The user to welcome.</param>
        /// <param name="channel">The channel in which the message should be send.</param>
        public async Task Welcome(IGuildUser user, ITextChannel channel = null)
        {
            var settings = await Load(user.GuildId);
            if (channel == null)
            {
                if (settings.ChannelId == null)
                    throw new InvalidWelcomeException("The welcome message is not enabled on this server.");

                channel = await user.Guild.GetTextChannelAsync(settings.ChannelId.Value);
                if (channel == null)
                    throw new InvalidChannelException("The channel does not exist in this server.");
            }

            var msg = string.Format(settings.Message, user.Mention);
            if (!settings.UseImage) await channel.SendMessageAsync(msg);
            else await channel.SendFileAsync(ImageExtensions.GetImagePath("GasaiYunoWelcome.jpg"), msg);
        }

        /// <summary>
        ///     Send a welcome message to a couple of users.
        /// </summary>
        /// <param name="channel">The channel in which the message should be send.</param>
        /// <param name="users">The users to welcome.</param>
        /// <returns></returns>
        public async Task Welcome(ITextChannel channel, params IGuildUser[] users)
        {
            var settings = await Load(channel.GuildId);

            var names = string.Join(", ", users.Select(u => u.Mention)).ReplaceLast(", ", " and ");
            var msg = string.Format(settings.Message, names);

            if (!settings.UseImage) await channel.SendMessageAsync(msg);
            else await channel.SendFileAsync(ImageExtensions.GetImagePath("GasaiYunoWelcome.jpg"), msg);
        }
    }
}