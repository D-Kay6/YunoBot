using Dal.Database.RavenDB.Models;
using Raven.Client.Documents.Indexes;
using System.Linq;

namespace Dal.Database.RavenDB.Indexes
{
    internal class Bans_ByUserId : AbstractIndexCreationTask<Ban>
    {
        public Bans_ByUserId()
        {
            Map = bans =>
                from ban in bans
                select new
                {
                    ban.UserId
                };
        }
    }
}