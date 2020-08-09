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
        private readonly IDbDynamicRoleData _dbData;
        private readonly IDbRoleIgnore _dbIgnore;
        
        public DynamicRoleService(IDbDynamicRole dbDynamic, IDbDynamicRoleData dbData, IDbRoleIgnore dbIgnore)
        {
            _dbDynamic = dbDynamic;
            _dbData = dbData;
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
        ///     Save the (new) settings for a dynamic role.
        /// </summary>
        /// <param name="settings">The settings for a dynamic role.</param>
        /// <exception cref="InvalidStatusException">Thrown if the status is null or empty.</exception>
        /// <exception cref="StatusExistsException">Thrown if the status is already in use for the given type.</exception>
        public async Task Save(DynamicRole settings)
        {
            if (string.IsNullOrWhiteSpace(settings.Status))
                throw new InvalidStatusException("The status of a dynamic role may not be empty.");

            var roles = await _dbDynamic.List(settings.ServerId);
            if (roles.Any(x => x.Type == settings.Type && x.Status.Equals(settings.Status, StringComparison.OrdinalIgnoreCase)))
                throw new StatusExistsException("The status must be unique for every dynamic type.");

            await _dbDynamic.Update(settings);
        }

        /// <summary>
        ///     Save the role data of a dynamic role..
        /// </summary>
        /// <param name="data">The role data to save.</param>
        /// <exception cref="DataExistsException">Thrown if the data for a dynamic role already exists.</exception>
        /// <exception cref="DataIncompleteException">Thrown if the data has missing id values.</exception>
        public async Task Save(DynamicRoleData data)
        {
            if (data.RoleId == 0)
                throw new DataIncompleteException("The id of the role is missing.");

            if (data.DynamicRoleId == 0)
                throw new DataIncompleteException("The id of the dynamic role is missing.");

            if (await _dbData.Get(data.RoleId, data.DynamicRoleId) != null)
                throw new DataExistsException("The role data already exists.");

            await _dbData.Add(data);
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