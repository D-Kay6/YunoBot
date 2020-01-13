using System;
using System.IO;
using System.Threading.Tasks;
using Dal.MySql;

namespace Dal.Json
{
    internal class Database : Json
    {
        private const string File = "Database.json";
        private string Directory => Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents", "Yuno Bot", "Configuration");

        public async Task<Connection> Read()
        {
            return await ReadAsync<Connection>(Directory, File);
        }

        public async Task Write(Connection data)
        {
            await WriteAsync(data, Directory, File);
        }
    }
}
