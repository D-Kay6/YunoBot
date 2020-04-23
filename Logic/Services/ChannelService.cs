namespace Logic.Services
{
    using Core.Entity;
    using Discord;
    using Exceptions;
    using IDal.Database;
    using System;
    using System.Threading.Tasks;

    public class ChannelService
    {
        private readonly IDbAutoChannel _dbAuto;
        private readonly IDbGeneratedChannel _dbGenerated;
        private readonly IDbPermaChannel _dbPerma;

        public ChannelService(IDbAutoChannel dbAuto, IDbGeneratedChannel dbGenerated, IDbPermaChannel dbPerma)
        {
            _dbAuto = dbAuto;
            _dbGenerated = dbGenerated;
            _dbPerma = dbPerma;
        }

        /// <summary>
        ///     Get the settings for auto channels.
        ///     Will reset corrupted values to default.
        /// </summary>
        /// <param name="serverId">The id of the server.</param>
        /// <returns>The auto channel settings.</returns>
        public async Task<AutoChannel> LoadAuto(ulong serverId)
        {
            var settings = await _dbAuto.Get(serverId);
            var update = false;

            if (string.IsNullOrWhiteSpace(settings.Prefix))
            {
                settings.Prefix = "➕";
                update = true;
            }

            if (string.IsNullOrWhiteSpace(settings.Name))
            {
                settings.Name = "--channel";
                update = true;
            }

            if (update)
                await _dbAuto.Update(settings);

            return settings;
        }

        /// <summary>
        ///     Get the settings for perma channels.
        ///     Will reset corrupted values to default.
        /// </summary>
        /// <param name="serverId">The id of the server.</param>
        /// <returns>The perma channel settings.</returns>
        public async Task<PermaChannel> LoadPerma(ulong serverId)
        {
            var settings = await _dbPerma.Get(serverId);
            var update = false;

            if (string.IsNullOrWhiteSpace(settings.Prefix))
            {
                settings.Prefix = "👥";
                update = true;
            }

            if (string.IsNullOrWhiteSpace(settings.Name))
            {
                settings.Name = "{0} channel";
                update = true;
            }

            if (update)
                await _dbPerma.Update(settings);

            return settings;
        }

        /// <summary>
        ///     Save the new settings for auto channels.
        /// </summary>
        /// <param name="settings">The new settings for auto channels.</param>
        /// <exception cref="InvalidPrefixException">Thrown if the prefix is null or empty.</exception>
        /// <exception cref="InvalidNameException">Thrown if the name is null or empty.</exception>
        /// <exception cref="PrefixExistsException">Thrown if the prefix is already in use by perma channels.</exception>
        public async Task Save(AutoChannel settings)
        {
            if (string.IsNullOrWhiteSpace(settings.Prefix))
                throw new InvalidPrefixException("The prefix cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(settings.Name))
                throw new InvalidNameException("The name cannot be null or empty.");

            var permaSettings = await _dbPerma.Get(settings.ServerId);
            if (permaSettings.Prefix.Equals(settings.Prefix, StringComparison.OrdinalIgnoreCase))
                throw new PrefixExistsException(
                    "The prefix for auto channels cannot be the same as the prefix for perma channels.");

            await _dbAuto.Update(settings);
        }

        /// <summary>
        ///     Save the new settings for perma channel.
        /// </summary>
        /// <param name="settings">The new settings for perma channels.</param>
        /// <exception cref="InvalidPrefixException">Thrown if the prefix is null or empty.</exception>
        /// <exception cref="InvalidNameException">Thrown if the name is null or empty.</exception>
        /// <exception cref="PrefixExistsException">Thrown if the prefix is already in use by auto channels.</exception>
        public async Task Save(PermaChannel settings)
        {
            if (string.IsNullOrWhiteSpace(settings.Prefix))
                throw new InvalidPrefixException("The prefix cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(settings.Name))
                throw new InvalidNameException("The name may not be null or empty.");

            var autoSettings = await _dbAuto.Get(settings.ServerId);
            if (autoSettings.Prefix.Equals(settings.Prefix, StringComparison.OrdinalIgnoreCase))
                throw new PrefixExistsException(
                    "The prefix for perma channels cannot be the same as the prefix for auto channels.");

            await _dbPerma.Update(settings);
        }

        /// <summary>
        ///     Register a generated channel.
        /// </summary>
        /// <param name="serverId">The id of the server.</param>
        /// <param name="channelId">The id of the channel.</param>
        /// <exception cref="ChannelExistsException">Thrown if the channel is already registered.</exception>
        public async Task AddGeneratedChannel(ulong serverId, ulong channelId)
        {
            var channel = await _dbGenerated.Get(serverId, channelId);
            if (channel != null)
                throw new ChannelExistsException("The generated channel already exists in the database.");

            channel = new GeneratedChannel
            {
                ServerId = serverId,
                ChannelId = channelId
            };
            await _dbGenerated.Add(channel);
        }

        /// <summary>
        ///     Check if a channel is a generated channel.
        /// </summary>
        /// <param name="channel">The channel to check.</param>
        /// <returns>True if the channel is generated by an auto channel.</returns>
        public async Task<bool> IsGeneratedChannel(IVoiceChannel channel)
        {
            var settings = await _dbGenerated.Get(channel.GuildId, channel.Id);
            return settings != null;
        }

        /// <summary>
        ///     Remove a generated channel.
        /// </summary>
        /// <param name="serverId">the id of the server.</param>
        /// <param name="channelId">the id of the channel.</param>
        /// <exception cref="InvalidChannelException">Thrown if the channel doesn't exist.</exception>
        public async Task RemoveGeneratedChannel(ulong serverId, ulong channelId)
        {
            var channel = await _dbGenerated.Get(serverId, channelId);
            if (channel == null)
                throw new InvalidChannelException("The generated channel doesn't exist in the database.");

            await _dbGenerated.Remove(channel);
        }
    }
}