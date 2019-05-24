using Dal.MySql;

namespace Dal.Json
{
    internal class Database : Json
    {
        private const string Directory = "Configuration";

        private const string File = "Database.json";

        public Connection Read()
        {
            return Read<Connection>(Directory, File);
        }

        public void Write(Connection data)
        {
            Write(data, Directory, File);
        }
    }
}
