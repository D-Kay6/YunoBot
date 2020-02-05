namespace Dal.Database
{
    internal abstract class Connection
    {
        public string Url;
        public string Ip;
        public string Port;
        public string Database;
        public string Username;
        public string Password;

        protected Connection()
        {
            this.Url = "";
            this.Port = "";
            this.Database = "";
            this.Username = "";
            this.Password = "";
        }

        public abstract string CreateConnectionString();
    }
}