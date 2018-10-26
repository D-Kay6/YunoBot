using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Yuno.Data.Serializer
{
    public class ObjectSerializer<T> where T : struct
    {
        private IFormatter _formatter;
        private string _directory;
        
        private string FullPath(string file) => Path.Combine(_directory, file);

        public ObjectSerializer(string directory)
        {
            _formatter = new BinaryFormatter();
            this._directory = directory;
        }

        public void SaveData(string file, T data)
        {
            if (!Directory.Exists(_directory)) Directory.CreateDirectory(_directory);
            using (Stream outStream = new FileStream(FullPath(file), FileMode.Create, FileAccess.Write, FileShare.None))
            {
                _formatter.Serialize(outStream, data);
            }
        }

        public T ReadData(string file)
        {
            var path = FullPath(file);
            if (!File.Exists(path)) return Activator.CreateInstance<T>();
            using (Stream inStream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
            {
                try
                {
                    return (T)_formatter.Deserialize(inStream);
                }
                catch (Exception e)
                {
                    Console.WriteLine("The data file is corrupt and will be replaced.");
                    var instance = Activator.CreateInstance<T>();
                    SaveData(file, instance);
                    return instance;
                }
            }
        }
    }
}