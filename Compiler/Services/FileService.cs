using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Compiler.Services
{
    public class FileService
    {
        public async Task<T> Read<T>(string path)
        {
            if (!File.Exists(path))
            {
                var data = Activator.CreateInstance<T>();
                await Write(data, path);
            }

            var json = await File.ReadAllTextAsync(path);
            return JsonConvert.DeserializeObject<T>(json);
        }

        public async Task Write<T>(T data, string path)
        {
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            await File.WriteAllTextAsync(path, json);
        }
    }
}