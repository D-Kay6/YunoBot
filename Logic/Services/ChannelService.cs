namespace Logic.Services
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading.Tasks;
    using Core.Entity;
    using Discord;
    using Exceptions;
    using IDal.Database;

    public class ChannelService
    {
        private readonly IDbAutoChannel _dbAuto;
        private readonly IDbPermaChannel _dbPerma;

        public ChannelService(IDbAutoChannel dbAuto, IDbPermaChannel dbPerma)
        {
            _dbAuto = dbAuto;
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
                throw new InvalidNameException("The name may not be null or empty.");

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
        ///     Check if a voice channel is an auto channel.
        ///     Either Auto channel or perma channel.
        /// </summary>
        /// <param name="channel">The channel to check.</param>
        /// <returns>True if the channel is an auto channel.</returns>
        public async Task<bool> IsAuto([NotNull] IVoiceChannel channel)
        {
            var autoSettings = await _dbAuto.Get(channel.GuildId);
            return channel.Name.StartsWith(autoSettings.Prefix, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     Check if a voice channel is a perma channel.
        /// </summary>
        /// <param name="channel">The channel to check.</param>
        /// <returns>True if the channel is a perma channel.</returns>
        public async Task<bool> IsPerma([NotNull] IVoiceChannel channel)
        {
            var permaSettings = await _dbPerma.Get(channel.GuildId);
            return channel.Name.StartsWith(permaSettings.Prefix, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     Get the auto channel prefix of a server.
        ///     Will reset the prefix to default if it is corrupted.
        /// </summary>
        /// <param name="serverId">The id of the server</param>
        /// <returns>The prefix for auto channels.</returns>
        public async Task<string> GetAutoPrefix(ulong serverId)
        {
            var autoSettings = await _dbAuto.Get(serverId);

            if (string.IsNullOrWhiteSpace(autoSettings.Prefix))
            {
                autoSettings.Prefix = "➕";
                await _dbAuto.Update(autoSettings);
            }

            return autoSettings.Prefix;
        }

        /// <summary>
        ///     Get the name used for channels generated by auto channels.
        /// </summary>
        /// <param name="serverId">The id of the server.</param>
        /// <returns>The name for generated channels.</returns>
        public async Task<string> GetAutoName(ulong serverId)
        {
            var autoSettings = await _dbAuto.Get(serverId);

            if (string.IsNullOrWhiteSpace(autoSettings.Name))
            {
                autoSettings.Name = "--channel";
                await _dbAuto.Update(autoSettings);
            }

            return autoSettings.Name;
        }

        /// <summary>
        ///     Change the prefix for auto channels.
        /// </summary>
        /// <param name="serverId">The id of the server.</param>
        /// <param name="prefix">the new prefix.</param>
        /// <exception cref="InvalidPrefixException">Thrown if the prefix is null or empty.</exception>
        /// <exception cref="PrefixExistsException">Thrown if the prefix is already in use by perma channels.</exception>
        public async Task SetAutoPrefix(ulong serverId, string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                throw new InvalidPrefixException("The prefix cannot be null or empty.");

            var permaSettings = await _dbPerma.Get(serverId);
            if (permaSettings.Prefix.Equals(prefix, StringComparison.OrdinalIgnoreCase))
                throw new PrefixExistsException(
                    "The prefix for auto channels cannot be the same as the prefix for perma channels.");

            var autoSettings = await _dbAuto.Get(serverId);
            autoSettings.Prefix = prefix;
            await _dbAuto.Update(autoSettings);
        }

        /// <summary>
        ///     Set the name channels generated by auto channels use.
        /// </summary>
        /// <param name="serverId">The id of the server.</param>
        /// <param name="name">The new name to be used for generated channels.</param>
        /// <exception cref="InvalidNameException">Thrown if the name is null or empty.</exception>
        public async Task SetAutoName(ulong serverId, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidNameException("The name may not be null or empty.");

            var autoSettings = await _dbAuto.Get(serverId);
            autoSettings.Name = name;
            await _dbAuto.Update(autoSettings);
        }

        /// <summary>
        ///     Get the perma channel prefix of a server.
        ///     Will reset the prefix to default if it is corrupted.
        /// </summary>
        /// <param name="serverId">The id of the server</param>
        /// <returns>The prefix for perma channels.</returns>
        public async Task<string> GetPermaPrefix(ulong serverId)
        {
            var permaSettings = await _dbPerma.Get(serverId);

            if (string.IsNullOrWhiteSpace(permaSettings.Prefix))
            {
                permaSettings.Prefix = "👥";
                await _dbPerma.Update(permaSettings);
            }

            return permaSettings.Prefix;
        }

        /// <summary>
        ///     Get the name used for channels generated by perma channels.
        /// </summary>
        /// <param name="serverId">The id of the server.</param>
        /// <returns>The name for generated channels.</returns>
        public async Task<string> GetPermaName(ulong serverId)
        {
            var permaSettings = await _dbPerma.Get(serverId);

            if (string.IsNullOrWhiteSpace(permaSettings.Name))
            {
                permaSettings.Name = "{0} channel";
                await _dbPerma.Update(permaSettings);
            }

            return permaSettings.Name;
        }

        /// <summary>
        ///     Change the prefix for perma channels.
        /// </summary>
        /// <param name="serverId">The id of the server.</param>
        /// <param name="prefix">the new prefix.</param>
        /// <exception cref="InvalidPrefixException">Thrown if the prefix is null or empty.</exception>
        /// <exception cref="PrefixExistsException">Thrown if the prefix is already in use by auto channels.</exception>
        public async Task SetPermaPrefix(ulong serverId, string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                throw new InvalidPrefixException("The prefix cannot be null or empty.");

            var autoSettings = await _dbAuto.Get(serverId);
            if (autoSettings.Prefix.Equals(prefix, StringComparison.OrdinalIgnoreCase))
                throw new PrefixExistsException(
                    "The prefix for perma channels cannot be the same as the prefix for auto channels.");

            var permaSettings = await _dbPerma.Get(serverId);
            permaSettings.Prefix = prefix;
            await _dbPerma.Update(permaSettings);
        }

        /// <summary>
        ///     Set the name channels generated by perma channels use.
        /// </summary>
        /// <param name="serverId">The id of the server.</param>
        /// <param name="name">The new name to be used for generated channels.</param>
        /// <exception cref="InvalidNameException">Thrown if the name is null or empty.</exception>
        public async Task SetPermaName(ulong serverId, string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new InvalidNameException("The name may not be null or empty.");

            var permaSettings = await _dbPerma.Get(serverId);
            permaSettings.Name = name;
            await _dbPerma.Update(permaSettings);
        }

        public async Task AddGeneratedChannel(ulong serverId, ulong channelId)
        {
            var settings = await _dbAuto.Get(serverId);
            if
        }
    }
}