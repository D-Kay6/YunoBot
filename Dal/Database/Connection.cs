namespace Dal.Database
{
    internal abstract class Connection
    {
        public string Database;
        public string Ip;
        public string Password;
        public string Port;
        public string Username;

        protected Connection()
        {
            Port = "";
            Database = "";
            Username = "";
            Password = "";
        }

        public abstract string CreateConnectionString();
    }
}