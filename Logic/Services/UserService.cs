﻿using Core.Entity;
using Discord;
using Discord.WebSocket;
using IDal.Database;
using Logic.Exceptions;
using System;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class UserService
    {
        private readonly DiscordShardedClient _client;
        private readonly IDbBan _dbBan;
        private readonly IDbUser _dbUser;

        public UserService(DiscordShardedClient client, IDbUser dbUser, IDbBan dbBan)
        {
            _client = client;
            _dbUser = dbUser;
            _dbBan = dbBan;
        }

        /// <summary>
        ///     Update the details of a user.
        ///     If the user does not exist yet, they are added.
        /// </summary>
        /// <param name="user">The user to update.</param>
        public async Task Update(IUser user)
        {
            var settings = await _dbUser.Get(user.Id);
            if (settings == null)
            {
                settings = new User
                {
                    Id = user.Id,
                    Name = user.Username
                };
                await _dbUser.Add(settings);
                return;
            }

            if (settings.Name != user.Username)
            {
                settings.Name = user.Username;
                await _dbUser.Update(settings);
            }
        }

        /// <summary>
        ///     Ban a user.
        /// </summary>
        /// <param name="user">The user to ban.</param>
        /// <param name="endDate">The date the ban should end.</param>
        /// <param name="reason">The reason for the ban.</param>
        public async Task Ban(IGuildUser user, DateTime endDate, string reason = null)
        {
            var settings = new Ban
            {
                UserId = user.Id,
                ServerId = user.GuildId,
                EndDate = endDate,
                Reason = reason
            };
            await _dbBan.Add(settings);
        }

        public Task Unban(Ban ban)
        {
            return _dbBan.Remove(ban);
        }
    }
}