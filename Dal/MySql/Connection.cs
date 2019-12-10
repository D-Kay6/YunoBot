namespace Dal.MySql
{
    internal class Connection
    {
        public string Ip;
        public string Port;
        public string Database;
        public string Username;
        public string Password;

        public Connection()
        {
            this.Ip = "";
            this.Port = "";
            this.Database = "";
            this.Username = "";
            this.Password = "";
        }

        public string CreateConnectionString()
        {
            return $"Server={Ip}; Database={Database}; Uid={Username}; Pwd={Password}; charset=utf8mb4;";
        }
    }
}
