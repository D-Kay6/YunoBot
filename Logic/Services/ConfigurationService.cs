using IDal;
using System.Threading.Tasks;
using Logic.Exceptions;

namespace Logic.Services
{
    public class ConfigurationService
    {
        private readonly IConfig _config;

        public ConfigurationService(IConfig config)
        {
            _config = config;
        }

        /// <summary>
        /// Read the token for the bot from the configuration file.
        /// </summary>
        /// <returns>The token used for the bot to login with.</returns>
        /// <exception cref="InvalidTokenException">Thrown if the token is not valid to use.</exception>
        public async Task<string> GetToken()
        {
            var data = await _config.Read();
            if (string.IsNullOrWhiteSpace(data.Token))
                throw new InvalidTokenException("The bot token in the configuration file is not valid.");

            return data.Token;
        }

        /// <summary>
        /// Read the token for DiscordBotsList from the configuration file.
        /// </summary>
        /// <returns>The token used for DiscordBotsList to login with.</returns>
        /// <exception cref="InvalidTokenException">Thrown if the token is not valid to use.</exception>
        public async Task<string> GetDblToken()
        {
            var data = await _config.Read();
            if (string.IsNullOrWhiteSpace(data.DiscordBotsToken))
                throw new InvalidTokenException("The token for DiscordBotsList in the configuration file is not valid.");

            return data.DiscordBotsToken;
        }
    }
}