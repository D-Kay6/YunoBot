using Dal.Database.RavenDB.Models;
using Raven.Client.Documents.Indexes;
using System.Linq;

namespace Dal.Database.RavenDB.Indexes
{
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