using Core.Entity;
using IDal.Database;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal.Database.MySql.EF.MultiThreaded
{
    public class BanRepository : MultiThreadedRepository, IDbBan
    {
        private readonly SingleThreaded.BanRepository _repository;

        public BanRepository()
        {
            _repository = new SingleThreaded.BanRepository();
        }

        public async Task<bool> IsBanned(ulong userId, ulong serverId)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.IsBanned(userId, serverId));
        }

        public async Task<bool> AddBan(ulong userId, ulong serverId, DateTime? endDate = null, string reason = null)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.AddBan(userId, serverId, endDate, reason));
        }

        public async Task<bool> RemoveBan(ulong userId, ulong serverId)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.RemoveBan(userId, serverId));
        }

        public async Task<bool> RemoveBan(Ban ban)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.RemoveBan(ban));
        }

        public async Task<Ban> GetBan(ulong userId, ulong serverId)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.GetBan(userId, serverId));
        }

        public async Task<List<Ban>> GetBans(bool expiredOnly = true)
        {
            await Semaphore.WaitAsync();
            return await Execute(_repository.GetBans(expiredOnly));
        }
    }
}