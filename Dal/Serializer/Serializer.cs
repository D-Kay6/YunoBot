using IDal.Interfaces;

namespace Dal.Serializer
{
    public class Serializer : ISerializer
    {
        private const string Directory = "Data\\Saves";

        private const string File = ".bin";

        private ObjectSerializer _serializer;

        public Serializer()
        {
            this._serializer = new ObjectSerializer();
        }
        
        public T Read<T>(ulong id)
        {
            return _serializer.ReadData<T>($"{Directory}\\{id}\\{typeof(T).Name}{File}");
        }

        public void Write<T>(ulong id, T data)
        {
            _serializer.SaveData($"{Directory}\\{id}\\{typeof(T).Name}{File}", data);
        }
    }
}
