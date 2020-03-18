namespace Dal.Json
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Database;

    internal class Database<T> : Json where T : Connection
    {
        private const string File = "Database.json";

        private string Directory => Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents",
            "Yuno Bot", "Configuration");

        public async Task<T> Read()
        {
            return await ReadAsync<T>(Directory, File);
        }

        public async Task Write(T data)
        {
            await WriteAsync(data, Directory, File);
        }
    }
}