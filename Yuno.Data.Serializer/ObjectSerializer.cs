using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Yuno.Data.Serializer
{
    public class ObjectSerializer
    {
        private IFormatter _formatter;

        public ObjectSerializer()
        {
            _formatter = new BinaryFormatter();
        }

        public void SaveData<T>(string path, T data)
        {
            var directory = Path.GetDirectoryName(path);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);
            using (Stream outStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                _formatter.Serialize(outStream, data);
            }
        }

        public T ReadData<T>(string path)
        {
            if (!File.Exists(path)) return default;
            using (Stream inStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite))
            {
                try
                {
                    return (T)_formatter.Deserialize(inStream);
                }
                catch (Exception e)
                {
                    Console.WriteLine($"The data file '{path}' is corrupt and will be removed.");
                    File.Delete(path);
                    return default;
                }
            }
        }
    }
}