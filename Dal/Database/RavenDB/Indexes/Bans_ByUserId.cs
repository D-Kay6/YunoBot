namespace Dal.Database.RavenDB.Indexes
{
    using System.Linq;
    using Models;
    using Raven.Client.Documents.Indexes;

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