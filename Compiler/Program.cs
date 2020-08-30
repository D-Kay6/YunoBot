using Compiler.Models;
using Compiler.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Compiler
{
    class Program
    {
        private static DirectoryInfo _source => new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), "Files"));
        private static DirectoryInfo _target => new DirectoryInfo(Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents", "Yuno Bot", "Localization"));

        private static readonly FileService FileService = new FileService();

        static async Task Main(string[] args)
        {
            foreach (var file in _source.EnumerateFiles())
            {
                var localization = await FileService.Read<Localization>(file.FullName);
                var compiledObject = new Localization
                {
                    Messages = Convert(localization.Messages),
                    UserPraises = localization.UserPraises,
                    GroupPraises = localization.GroupPraises
                };
                await FileService.Write(compiledObject, Path.Combine(_target.FullName, file.Name));
            }
        }

        static Dictionary<string, object> Convert(Dictionary<string, object> collection, string category = null)
        {
            category = category == null ? string.Empty : $"{category} ";

            var result = new Dictionary<string, object>();
            foreach (var item in collection)
            {
                string name = category + item.Key;
                if (item.Value is string)
                {
                    result.Add(name, item.Value);
                }
                else
                {
                    var jObject = (JObject)item.Value;
                    var conversion = Convert(jObject.ToObject<Dictionary<string, object>>(), name);
                    foreach (var convertedObject in conversion)
                    {
                        result.Add(convertedObject.Key, convertedObject.Value);
                    }
                }
            }
            return result;
        }
    }
}
