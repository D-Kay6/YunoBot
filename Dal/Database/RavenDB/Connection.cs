namespace Dal.Database.RavenDB
{
    internal class Connection : Database.Connection
    {
        public override string CreateConnectionString()
        {
            return $"Url = {Url};Database={Database};User={Username};Password={Password}";
        }
    }
}