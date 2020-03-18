namespace Logic.Services
{
    using Core.Entity;
    using Exceptions;
    using IDal.Database;
    using System;
    using System.Threading.Tasks;

    public class RoleService
    {
        private readonly IDbAutoRole _dbAuto;
        private readonly IDbPermaRole _dbPerma;
        private readonly IDbRoleIgnore _dbIgnore;

        public RoleService(IDbAutoRole dbAuto, IDbPermaRole dbPerma, IDbRoleIgnore dbIgnore)
        {
            _dbAuto = dbAuto;
            _dbPerma = dbPerma;
            _dbIgnore = dbIgnore;
        }

        /// <summary>
        ///     Get the settings for auto roles.
        ///     Will reset corrupted values to default.
        /// </summary>
        /// <param name="serverId">The id of the server.</param>
        /// <returns>The auto role settings.</returns>
        public async Task<AutoRole> LoadAuto(ulong serverId)
        {
            var settings = await _dbAuto.Get(serverId);
            var update = false;

            if (string.IsNullOrWhiteSpace(settings.Prefix))
            {
                settings.Prefix = "👾";
                update = true;
            }

            if (update)
                await _dbAuto.Update(settings);

            return settings;
        }

        /// <summary>
        ///     Get the settings for perma roles.
        ///     Will reset corrupted values to default.
        /// </summary>
        /// <param name="serverId">The id of the server.</param>
        /// <returns>The perma role settings.</returns>
        public async Task<PermaRole> LoadPerma(ulong serverId)
        {
            var settings = await _dbPerma.Get(serverId);
            var update = false;

            if (string.IsNullOrWhiteSpace(settings.Prefix))
            {
                settings.Prefix = "🎮";
                update = true;
            }

            if (update)
                await _dbPerma.Update(settings);

            return settings;
        }

        /// <summary>
        ///     Save the new settings for auto roles.
        /// </summary>
        /// <param name="settings">The new settings for auto roles.</param>
        /// <exception cref="InvalidPrefixException">Thrown if the prefix is null or empty.</exception>
        /// <exception cref="PrefixExistsException">Thrown if the prefix is already in use by perma roles.</exception>
        public async Task Save(AutoRole settings)
        {
            if (string.IsNullOrWhiteSpace(settings.Prefix))
                throw new InvalidPrefixException("The prefix cannot be null or empty.");

            var permaSettings = await _dbPerma.Get(settings.ServerId);
            if (permaSettings.Prefix.Equals(settings.Prefix, StringComparison.OrdinalIgnoreCase))
                throw new PrefixExistsException(
                    "The prefix for auto roles cannot be the same as the prefix for perma roles.");

            await _dbAuto.Update(settings);
        }

        /// <summary>
        ///     Save the new settings for perma role.
        /// </summary>
        /// <param name="settings">The new settings for perma roles.</param>
        /// <exception cref="InvalidPrefixException">Thrown if the prefix is null or empty.</exception>
        /// <exception cref="PrefixExistsException">Thrown if the prefix is already in use by auto roles.</exception>
        public async Task Save(PermaRole settings)
        {
            if (string.IsNullOrWhiteSpace(settings.Prefix))
                throw new InvalidPrefixException("The prefix cannot be null or empty.");

            var autoSettings = await _dbAuto.Get(settings.ServerId);
            if (autoSettings.Prefix.Equals(settings.Prefix, StringComparison.OrdinalIgnoreCase))
                throw new PrefixExistsException(
                    "The prefix for perma roles cannot be the same as the prefix for auto roles.");

            await _dbPerma.Update(settings);
        }

        /// <summary>
        ///     Register a user to ignore role automation.
        /// </summary>
        /// <param name="serverId">The id of the server.</param>
        /// <param name="userId">The id of the user.</param>
        /// <exception cref="RoleIgnoreExistsException">Thrown if the user is already registered.</exception>
        public async Task AddRoleIgnore(ulong serverId, ulong userId)
        {
            var ignore = await _dbIgnore.Get(serverId, userId);
            if (ignore != null)
                throw new RoleIgnoreExistsException("The generated role already exists in the database.");

            ignore = new RoleIgnore
            {
                ServerId = serverId,
                UserId = userId
            };
            await _dbIgnore.Add(ignore);
        }

        /// <summary>
        ///     Check if a user is set to ignore role automation.
        /// </summary>
        /// <param name="serverId">The id of the server.</param>
        /// <param name="userId">The id of the user.</param>
        /// <returns>True if the user is to ignore role automation.</returns>
        public async Task<bool> IsRoleIgnore(ulong serverId, ulong userId)
        {
            var ignore = await _dbIgnore.Get(serverId, userId);
            return ignore != null;
        }

        /// <summary>
        ///     Remove the ignore settings for a user.
        /// </summary>
        /// <param name="serverId">the id of the server.</param>
        /// <param name="userId">the id of the user.</param>
        /// <exception cref="InvalidRoleIgnoreException">thrown if the settings don't exist.</exception>
        public async Task RemoveRoleIgnore(ulong serverId, ulong userId)
        {
            var ignore = await _dbIgnore.Get(serverId, userId);
            if (ignore == null)
                throw new InvalidRoleIgnoreException("The role ignore settings don't exist in the database.");

            await _dbIgnore.Remove(ignore);
        }

        /* These are methods I might delete as they have no real use, other than being buffers for the conversion.
        /// <summary>
        ///     Check if a voice role is an auto role.
        ///     Either Auto role or perma role.
        /// </summary>
        /// <param name="role">The role to check.</param>
        /// <returns>True if the role is an auto role.</returns>
        public async Task<bool> IsAuto([NotNull] IRole role)
        {
            var autoSettings = await _dbAuto.Get(role.Guild.Id);
            return role.Name.StartsWith(autoSettings.Prefix, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     Check if a voice role is a perma role.
        /// </summary>
        /// <param name="role">The role to check.</param>
        /// <returns>True if the role is a perma role.</returns>
        public async Task<bool> IsPerma([NotNull] IRole role)
        {
            var permaSettings = await _dbPerma.Get(role.Guild.Id);
            return role.Name.StartsWith(permaSettings.Prefix, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     Get the auto role prefix of a server.
        ///     Will reset the prefix to default if it is corrupted.
        /// </summary>
        /// <param name="serverId">The id of the server</param>
        /// <returns>The prefix for auto roles.</returns>
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
        ///     Change the prefix for auto roles.
        /// </summary>
        /// <param name="serverId">The id of the server.</param>
        /// <param name="prefix">the new prefix.</param>
        /// <exception cref="InvalidPrefixException">Thrown if the prefix is null or empty.</exception>
        /// <exception cref="PrefixExistsException">Thrown if the prefix is already in use by perma roles.</exception>
        public async Task SetAutoPrefix(ulong serverId, string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                throw new InvalidPrefixException("The prefix cannot be null or empty.");

            var permaSettings = await _dbPerma.Get(serverId);
            if (permaSettings.Prefix.Equals(prefix, StringComparison.OrdinalIgnoreCase))
                throw new PrefixExistsException(
                    "The prefix for auto roles cannot be the same as the prefix for perma roles.");

            var autoSettings = await _dbAuto.Get(serverId);
            autoSettings.Prefix = prefix;
            await _dbAuto.Update(autoSettings);
        }

        /// <summary>
        ///     Get the perma role prefix of a server.
        ///     Will reset the prefix to default if it is corrupted.
        /// </summary>
        /// <param name="serverId">The id of the server</param>
        /// <returns>The prefix for perma roles.</returns>
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
        ///     Change the prefix for perma roles.
        /// </summary>
        /// <param name="serverId">The id of the server.</param>
        /// <param name="prefix">the new prefix.</param>
        /// <exception cref="InvalidPrefixException">Thrown if the prefix is null or empty.</exception>
        /// <exception cref="PrefixExistsException">Thrown if the prefix is already in use by auto roles.</exception>
        public async Task SetPermaPrefix(ulong serverId, string prefix)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                throw new InvalidPrefixException("The prefix cannot be null or empty.");

            var autoSettings = await _dbAuto.Get(serverId);
            if (autoSettings.Prefix.Equals(prefix, StringComparison.OrdinalIgnoreCase))
                throw new PrefixExistsException(
                    "The prefix for perma roles cannot be the same as the prefix for auto roles.");

            var permaSettings = await _dbPerma.Get(serverId);
            permaSettings.Prefix = prefix;
            await _dbPerma.Update(permaSettings);
        }
        */
    }
}