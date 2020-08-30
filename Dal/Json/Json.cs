using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Dal.Json
{
    public abstract class Json
    {
        protected async Task<T> ReadAsync<T>(string directory, string file)
        {
            if (string.IsNullOrWhiteSpace(file)) return default;

            var path = string.IsNullOrWhiteSpace(directory) ? file : Path.Combine(directory, file);
            if (!File.Exists($"{path}"))
            {
                var data = Activator.CreateInstance<T>();
                await WriteAsync(data, directory, file);
            }

            var json = await File.ReadAllTextAsync($"{path}");
            return JsonConvert.DeserializeObject<T>(json);
        }

        protected async Task WriteAsync<T>(T data, string directory, string file)
        {
            if (string.IsNullOrWhiteSpace(file)) return;

            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            var path = string.IsNullOrWhiteSpace(directory) ? file : Path.Combine(directory, file);
            await File.WriteAllTextAsync(path, json);
        }
    }
}