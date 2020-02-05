namespace Dal.Database.MySql
{
    internal class Connection : Database.Connection
    {
        public override string CreateConnectionString()
        {
            return $"Server={Ip}; Database={Database}; Uid={Username}; Pwd={Password}; charset=utf8mb4;";
        }
    }
}
