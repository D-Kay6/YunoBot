using Core.Entity;
using Core.Enum;
using IDal.Database;
using Logic.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class DynamicRoleService
    {
        private readonly IDbDynamicRole _dbDynamic;
        private readonly IDbRoleIgnore _dbIgnore;
        
        public DynamicRoleService(IDbDynamicRole dbDynamic, IDbRoleIgnore dbIgnore)
        {
            _dbDynamic = dbDynamic;
            _dbIgnore = dbIgnore;
        }

        public async Task<DynamicRole> Get(ulong serverId, string status)
        {
            var dynamicRoles = await _dbDynamic.List(serverId);
            var dynamicRole = dynamicRoles.FirstOrDefault(x => x.Status.Equals(status, StringComparison.OrdinalIgnoreCase));
            return dynamicRole;
        }

        public async Task<List<DynamicRole>> Find(ulong serverId, string status, AutomationType? type = null)
        {
            var dynamicRoles = await _dbDynamic.List(serverId);
            var results = dynamicRoles.Where(x => status.Contains(x.Status, StringComparison.OrdinalIgnoreCase));
            if (type != null)
                results = results.Where(x => x.Type == type);

            return results.ToList();
        }

        /// <summary>
        ///     Get the settings for auto roles.
        ///     Will reset corrupted values to default.
        /// </summary>
        /// <param name="serverId">The id of the server.</param>
        /// <returns>The auto role settings.</returns>
        public async Task<DynamicRole> Load(ulong serverId)
        {
            //var settings = await _dbDynamic.Get(serverId);

            //if (settings == null)
            //{
            //    settings = new AutoRole
            //    {
            //        ServerId = serverId
            //    };
            //    await _dbAuto.Add(settings);
            //    return settings;
            //}

            //var update = false;
            //if (string.IsNullOrWhiteSpace(settings.Prefix))
            //{
            //    settings.Prefix = "👾";
            //    update = true;
            //}

            //if (update)
            //    await _dbAuto.Update(settings);

            //return settings;
            return null;
        }

        /// <summary>
        ///     Get the settings for perma roles.
        ///     Will reset corrupted values to default.
        /// </summary>
        /// <param name="serverId">The id of the server.</param>
        /// <returns>The perma role settings.</returns>
        public async Task<DynamicRole> LoadPerma(ulong serverId)
        {
            //var settings = await _dbPerma.Get(serverId);

            //if (settings == null)
            //{
            //    settings = new PermaRole
            //    {
            //        ServerId = serverId
            //    };
            //    await _dbPerma.Add(settings);
            //    return settings;
            //}

            //var update = false;
            //if (string.IsNullOrWhiteSpace(settings.Prefix))
            //{
            //    settings.Prefix = "🎮";
            //    update = true;
            //}

            //if (update)
            //    await _dbPerma.Update(settings);

            //return settings;
            return null;
        }

        /// <summary>
        ///     Save the new settings for auto roles.
        /// </summary>
        /// <param name="settings">The new settings for auto roles.</param>
        /// <exception cref="InvalidPrefixException">Thrown if the prefix is null or empty.</exception>
        /// <exception cref="PrefixExistsException">Thrown if the prefix is already in use by perma roles.</exception>
        public async Task Save(DynamicRole settings)
        {

        }

        /// <summary>
        ///     Save the new settings for auto roles.
        /// </summary>
        /// <param name="settings">The new settings for auto roles.</param>
        /// <exception cref="InvalidPrefixException">Thrown if the prefix is null or empty.</exception>
        /// <exception cref="PrefixExistsException">Thrown if the prefix is already in use by perma roles.</exception>
        public async Task Save(DynamicRoleData data)
        {
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

            ignore = new DynamicRoleIgnore
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
    }
}