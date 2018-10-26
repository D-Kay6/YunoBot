using System.IO;
using Yuno.Data.Core.Interfaces;
using Yuno.Data.Core.Structs;

namespace Yuno.Data.Serializer
{
    public class Serializer : ISerializer
    {
        private const string Directory = "Data";
        private const string File = ".bin";
        private string FullPath => Path.Combine(Directory, File);

        private ObjectSerializer<Persistence> _serializer;

        public Serializer()
        {
            this._serializer = new ObjectSerializer<Persistence>(Directory);
        }

        public Persistence Read(ulong id)
        {
            return _serializer.ReadData($"{id}{File}");
        }

        public void Write(ulong id, Persistence data)
        {
            _serializer.SaveData($"{id}{File}", data);
        }
    }
}
