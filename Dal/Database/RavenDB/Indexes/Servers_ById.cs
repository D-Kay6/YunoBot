using Entity.RavenDB;
using Raven.Client.Documents.Indexes;
using System.Linq;
using Server = Dal.Database.RavenDB.Models.Server;

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
