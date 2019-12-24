using System;
using System.IO;
using System.Threading.Tasks;

namespace Dal.Text
{
    internal class TextStream
    {
        public async Task WriteAsync(string path, string data)
        {
            using (var stream = new StreamWriter(path))
            {
                await stream.WriteAsync(data);
            }
        }

        public async Task WriteLineAsync(string path, string data)
        {
            await WriteAsync(path, data, true);
        }

        public async Task OverwriteAsync(string path, string data)
        {
            await WriteAsync(path, data, false);
        }

        private async Task WriteAsync(string path, string data, bool append)
        {
            using (var stream = new StreamWriter(path, append))
            {
                await stream.WriteLineAsync(data);
            }
        }

        public async Task<T> ReadAsync<T>(string path)
        {
            using (var stream = new StreamReader(path))
            {
                var text = await stream.ReadToEndAsync();
                return (T)Convert.ChangeType(text, typeof(T));
            }
        }
    }
}
