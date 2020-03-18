namespace Dal.Database.RavenDB.Indexes
{
    using System.Linq;
    using Models;
    using Raven.Client.Documents.Indexes;

    internal class Servers_ById : AbstractIndexCreationTask<Server>
    {
        public Servers_ById()
        {
            Map = servers =>
                from server in servers
                select new
                {
                    server.Id,
                    server.ServerId
                };
        }
    }
}