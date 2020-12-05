using Core.Entity;
using IDal.Database;
using IDal.Exceptions;
using Logic.Exceptions;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class CommandService
    {
        private readonly IDbCommandCustom _dbCustom;
        private readonly IDbCommandSetting _dbSetting;

        public CommandService(IDbCommandSetting dbSetting, IDbCommandCustom dbCustom)
        {
            _dbSetting = dbSetting;
            _dbCustom = dbCustom;
        }

        /// <summary>
        ///     Set the prefix of a server.
        /// </summary>
        /// <param name="serverId">The id of the server.</param>
        /// <param name="prefix">the new prefix.</param>
        public async Task SetPrefix(ulong serverId, string prefix)
        {
            var setting = new CommandSetting
            {
                ServerId = serverId,
                Prefix = prefix
            };
            await _dbSetting.Update(setting);
        }

        /// <summary>
        ///     Get the prefix for a server.
        ///     Will reset the prefix to default if it is corrupted.
        /// </summary>
        /// <param name="serverId">The id of the server.</param>
        /// <returns>The prefix used for the server.</returns>
        /// <exception cref="UnknownServerException">Thrown if the main server data could not be found.</exception>
        public async Task<string> GetPrefix(ulong serverId)
        {
            var result = await _dbSetting.Get(serverId);

            if (result == null)
            {
                result = new CommandSetting
                {
                    ServerId = serverId
                };
                try
                {
                    await _dbSetting.Add(result);
                }
                catch (Exception e)
                {
                    throw new UnknownServerException("The database is most likely missing the main server data.", e);
                }
            }

            if (string.IsNullOrWhiteSpace(result?.Prefix))
            {
                result.Prefix = "/";
                await _dbSetting.Update(result);
            }

            return result.Prefix;
        }

        /// <summary>
        ///     Add a custom command to a server.
        /// </summary>
        /// <param name="serverId">The id of the server.</param>
        /// <param name="command">The command key.</param>
        /// <param name="response">The response of the command.</param>
        /// <exception cref="CommandExistsException">Thrown if the command already exists.</exception>
        public async Task AddCustomCommand(ulong serverId, string command, string response)
        {
            try
            {
                await _dbCustom.Add(new CustomCommand
                {
                    ServerId = serverId,
                    Command = command,
                    Response = response
                });
            }
            catch (ItemExistsException e)
            {
                throw new CommandExistsException(e.Message);
            }
        }

        /// <summary>
        ///     Remove a custom command from a server.
        /// </summary>
        /// <param name="serverId">the id of the server.</param>
        /// <param name="command">The command key.</param>
        /// <exception cref="InvalidCommandException">thrown if the command does not exist.</exception>
        public async Task RemoveCustomCommand(ulong serverId, string command)
        {
            try
            {
                await _dbCustom.Remove(new CustomCommand
                {
                    ServerId = serverId,
                    Command = command
                });
            }
            catch (InvalidItemException e)
            {
                throw new InvalidCommandException(e.Message, e);
            }
        }

        /// <summary>
        ///     Get the response for a custom command of a server.
        /// </summary>
        /// <param name="serverId">the id of the server.</param>
        /// <param name="command">The command key.</param>
        /// <returns>The response of the command.</returns>
        /// <exception cref="InvalidCommandException">thrown if the command does not exist.</exception>
        public async Task<string> GetResponse(ulong serverId, string command)
        {
            var result = await _dbCustom.Get(serverId, command);
            if (result == null)
                throw new InvalidCommandException("There is no custom command with that key for the provided server.");

            return result.Response;
        }

        /// <summary>
        ///     Get a list of all custom commands of a server.
        /// </summary>
        /// <param name="serverId">The id of the server.</param>
        /// <returns>A list of all custom commands.</returns>
        public async Task<List<CustomCommand>> GetAllCustom(ulong serverId)
        {
            return await _dbCustom.List(serverId);
        }
    }
}