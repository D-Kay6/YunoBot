using System;
using System.IO;
using Dal.MySql;

namespace Dal.Json
{
    internal class Database : Json
    {
        private const string File = "Database.json";
        private string Directory => Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents", "Yuno Bot", "Configuration");

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
