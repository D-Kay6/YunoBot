using System;
using System.IO;
using Newtonsoft.Json;

namespace Yuno.Data.Json
{
    public abstract class Json
    {
        protected T Read<T>(string directory, string file)
        {
            if (string.IsNullOrWhiteSpace(file)) return default(T);

            var path = string.IsNullOrWhiteSpace(directory) ? file : Path.Combine(directory, file);
            if (!File.Exists($"{path}"))
            {
                var data = Activator.CreateInstance<T>();
                Write(data, directory, file);
            }
            
            var json = File.ReadAllText($"{path}");
            return JsonConvert.DeserializeObject<T>(json);
        }

        protected void Write<T>(T data, string directory, string file)
        {
            if (string.IsNullOrWhiteSpace(file)) return;
            
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            var path = string.IsNullOrWhiteSpace(directory) ? file : Path.Combine(directory, file);
            File.WriteAllText(path, json);
        }
    }
}
