using Dal.Database.RavenDB.Indexes;
using Entity.RavenDB;
using IDal.Database.Raven;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal.Database.RavenDB.Repositories
{
    public class BanRepository : BaseRepository, IDbBan
    {
        public async Task AddBan(Ban value, Server server)
        {
            using var session = Context.GetAsyncSession();
            await session.StoreAsync(value);
            await session.SaveChangesAsync();
        }

        public async Task UpdateBan(Ban value)
        {
            using var session = Context.GetAsyncSession();
            var ban = await session.LoadAsync<Ban>(value.Id);
            ban.EndDate = value.EndDate;
            ban.Reason = value.Reason;
            await session.SaveChangesAsync();
        }

        public async Task RemoveBan(string id)
        {
            using var session = Context.GetAsyncSession();
            session.Delete(id);
        }

        public async Task<Ban> GetBan(ulong userId, ulong serverId)
        {
            using var session = Context.GetAsyncSession();
            var bans = session.Query<Ban, Bans_ByUserId>()
                .Include(x => x.ServerId)
                .Where(x => x.UserId == userId);
            return null;
        }

        public async Task<List<Ban>> GetBans(bool expiredOnly = true)
        {
            throw new NotImplementedException();
        }
    }
}