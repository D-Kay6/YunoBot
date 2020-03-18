namespace Dal.Database
{
    internal abstract class Connection
    {
        public string Database;
        public string Ip;
        public string Password;
        public string Port;
        public string Url;
        public string Username;

        protected Connection()
        {
            Url = "";
            Port = "";
            Database = "";
            Username = "";
            Password = "";
        }

        public abstract string CreateConnectionString();
    }
}