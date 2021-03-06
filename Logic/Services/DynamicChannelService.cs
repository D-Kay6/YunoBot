﻿using Core.Entity;
using Core.Enum;
using Discord;
using IDal.Database;
using Logic.Exceptions;
using Raven.Client.Documents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class DynamicChannelService
    {
        private readonly IDbDynamicChannel _dbDynamic;
        private readonly IDbGeneratedChannel _dbGenerated;

        public DynamicChannelService(IDbDynamicChannel dbDynamic, IDbGeneratedChannel dbGenerated)
        {
            _dbDynamic = dbDynamic;
            _dbGenerated = dbGenerated;
        }

        /// <summary>
        ///     Get the settings for dynamic channels.
        ///     Will reset corrupted values to default.
        /// </summary>
        /// <param name="serverId">The id of the server.</param>
        /// <param name="type">The type of the dynamic settings.</param>
        /// <returns>The dynamic channel settings.</returns>
        public async Task<DynamicChannel> Load(ulong serverId, AutomationType type)
        {
            var channels = await _dbDynamic.List(serverId);
            var settings = channels.FirstOrDefault(x => x.Type == type);
            if (settings == null)
            {
                settings = new DynamicChannel(type)
                {
                    ServerId = serverId
                };
                await _dbDynamic.Add(settings);
                return settings;
            }

            var update = false;
            if (string.IsNullOrWhiteSpace(settings.Prefix))
            {
                switch (type)
                {
                    case AutomationType.Temporary:
                        settings.Prefix = "➕";
                        break;
                    case AutomationType.Permanent:
                        settings.Prefix = "👥";
                        break;
                }
                update = true;
            }

            if (string.IsNullOrWhiteSpace(settings.Name))
            {
                switch (type)
                {
                    case AutomationType.Temporary:
                        settings.Name = "--channel";
                        break;
                    case AutomationType.Permanent:
                        settings.Name = "{0} channel";
                        break;
                }
                update = true;
            }

            if (update)
                await _dbDynamic.Update(settings);

            return settings;
        }

        /// <summary>
        ///     Save the new settings for dynamic channels.
        /// </summary>
        /// <param name="settings">The new settings for dynamic channels.</param>
        /// <exception cref="InvalidPrefixException">Thrown if the prefix is null or empty.</exception>
        /// <exception cref="InvalidNameException">Thrown if the name is null or empty.</exception>
        /// <exception cref="PrefixExistsException">Thrown if the prefix is already in use by perma channels.</exception>
        public async Task Save(DynamicChannel settings)
        {
            if (string.IsNullOrWhiteSpace(settings.Prefix))
                throw new InvalidPrefixException("The prefix cannot be null or empty.");

            if (string.IsNullOrWhiteSpace(settings.Name))
                throw new InvalidNameException("The name cannot be null or empty.");

            var channels = await _dbDynamic.List(settings.ServerId);
            if (channels.Any(x => 
                x.Prefix.Equals(settings.Prefix, StringComparison.OrdinalIgnoreCase) && 
                x.Id != settings.Id)) 
                throw new PrefixExistsException("The prefix for auto channels cannot be the same as the prefix for perma channels.");

            await _dbDynamic.Update(settings);
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
        /// <param name="serverId">The id of the server.</param>
        /// <param name="channelId">The id of the channel.</param>
        /// <exception cref="InvalidChannelException">Thrown if the channel doesn't exist.</exception>
        public async Task RemoveGeneratedChannel(ulong serverId, ulong channelId)
        {
            var channel = await _dbGenerated.Get(serverId, channelId);
            if (channel == null)
                throw new InvalidChannelException("The generated channel doesn't exist in the database.");

            await _dbGenerated.Remove(channel);
        }

        /// <summary>
        ///     Remove a generated channel.
        /// </summary>
        /// <param name="channel">tThe generated channel object.</param>
        /// <exception cref="InvalidChannelException">Thrown if the channel doesn't exist.</exception>
        public async Task RemoveGeneratedChannel(GeneratedChannel channel)
        {
            if (channel == null || channel.ServerId == 0 || channel.ChannelId == 0)
                throw new InvalidChannelException("The generated channel doesn't exist in the database.");

            await _dbGenerated.Remove(channel);
        }

        /// <summary>
        ///     List all the generated channels of a server.
        /// </summary>
        /// <param name="serverId">The id of the server.</param>
        /// <returns>The list of generated channels.</returns>
        public Task<List<GeneratedChannel>> ListGeneratedChannels(ulong serverId)
        {
            return _dbGenerated.Query(serverId).ToListAsync();
        }
    }
}