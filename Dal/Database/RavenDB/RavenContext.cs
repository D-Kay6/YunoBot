using Dal.Json;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Dal.Database.RavenDB
{
    internal class RavenContext
    {
        private readonly X509Certificate2 certificate;

        private readonly Lazy<IDocumentStore> store;
        private Connection connection;

        public RavenContext()
        {
            try
            {
                certificate = new X509Certificate2(Path.Combine(Directory, File), string.Empty,
                    X509KeyStorageFlags.UserKeySet);
            }
            catch (Exception)
            {
                //ignore
            }

            Task.Run(async () =>
            {
                var database = new Database<Connection>();
                connection = await database.Read();
            }).Wait();

            store = new Lazy<IDocumentStore>(() =>
            {
                var store = new DocumentStore
                {
                    Certificate = certificate,
                    Urls = new[] {connection.Url},
                    Database = connection.Database
                };

                return store.Initialize();
            });
        }

        private static string File => "Certificate.pfx";

        private static string Directory => Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"),
            "Documents", "Yuno Bot", "Certificate");

        private IDocumentStore Store => store.Value;

        public IDocumentSession GetSession()
        {
            return Store.OpenSession();
        }

        public IAsyncDocumentSession GetAsyncSession()
        {
            return Store.OpenAsyncSession();
        }
    }
}